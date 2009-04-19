﻿/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Mosa.Tools.TransformMonoSource
{
	class Program
	{
		class ClassNode
		{
			public int Start;
			public int End;
			public int Declare;
			public List<ClassNode> Children = new List<ClassNode>();
			public List<MethodNode> Methods = new List<MethodNode>();
			public ClassNode Parent;
			public bool Partial = false;
			public string Name = string.Empty;
			public List<int> OtherDeclare = new List<int>();

			public ClassNode()
			{
				this.Parent = this; // trick!
				this.Start = int.MinValue;
				this.End = int.MaxValue;
			}

			public ClassNode(ClassNode parent, int start, int end, int declare)
			{
				this.Parent = parent;
				this.Start = start;
				this.End = end;
				this.Declare = declare;
			}
		}

		class MethodNode
		{
			public ClassNode ClassNode;
			public int Declare;
			public int Start;
			public int End;

			public MethodNode(ClassNode classNode, int start, int end, int declare)
			{
				this.ClassNode = classNode;
				this.Declare = declare;
				this.Start = start;
				this.End = end;
			}
		}

		/// <summary>
		/// Mains the specified args.
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		static int Main(string[] args)
		{
			Console.WriteLine("TransformMonoSource v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2009. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();
			Console.WriteLine("Usage: TransformMonoSource <source directory> <destination directory>");
			Console.WriteLine();

			if (args.Length < 2) {
				Console.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			try {
				List<string> files = new List<string>();

				FindFiles(args[0], string.Empty, ref files);

				foreach (string file in files)
					Process(args[0], file, args[1]);
			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

		static void FindFiles(string root, string directory, ref List<string> files)
		{
			foreach (string file in Directory.GetFiles(Path.Combine(root, directory), "*.cs", SearchOption.TopDirectoryOnly)) {
				files.Add(Path.Combine(directory, Path.GetFileName(file)));
			}

			foreach (string dir in Directory.GetDirectories(Path.Combine(root, directory), "*.*", SearchOption.TopDirectoryOnly)) {
				if (dir.Contains(".svn"))
					continue;
				FindFiles(root, Path.Combine(directory, Path.GetFileName(dir)), ref files);
			}
		}

		static void Process(string root, string filename, string dest)
		{
			// Load file into string array
			string[] lines = File.ReadAllLines(Path.Combine(root, filename));

			ClassNode rootNode = new ClassNode();
			List<ClassNode> classNodes = new List<ClassNode>();
			List<MethodNode> methodNodes = new List<MethodNode>();

			List<int> namespaces = new List<int>();
			List<int> usings = new List<int>();

			ClassNode currentNode = rootNode;

			// Analyze File
			for (int linenbr = 0; linenbr < lines.Length; linenbr++) {
				string line = lines[linenbr];

				int comment = line.IndexOf("//");
				if (comment > 0)
					line = line.Substring(0, comment + 2);

				int colon = lines[linenbr].IndexOf(":");
				int skip = 0;

				if ((line.Contains(" extern ") || (line.Contains("\textern ")) || (line.StartsWith("extern "))) && !line.Contains(" alias ")) {
					int start = GetPreviousBlockEnd(lines, linenbr);
					skip = GetNumberOfMethodDeclarationLines(lines, linenbr, false);
					MethodNode node = new MethodNode(currentNode, start, linenbr + skip, linenbr);
					methodNodes.Add(node);
					currentNode.Methods.Add(node);
				}
				else if (line.StartsWith("using ") && line.Contains(";")) {
					usings.Add(linenbr);
				}
				else if (line.StartsWith("namespace ")) {
					namespaces.Add(linenbr);
				}
				else if (line.Contains(" class ") || (line.Contains("\tclass ")) || (line.StartsWith("class ")) ||
						line.Contains(" struct ") || (line.Contains("\tstruct ")) || (line.StartsWith("struct "))) {

					// Attempt to include keywords in quotes
					if (line.Contains("\""))
						continue;

					string className = GetClassName(lines, linenbr);

					// Attempt to handle #else class definitions
					if (className == currentNode.Name) {
						currentNode.OtherDeclare.Add(linenbr);
						continue;
					}

					// Search backwards for the start of the class definition (might not be on the same line as class keyword)
					int start = GetPreviousBlockEnd(lines, linenbr);

					// Find the last line of the class
					int end = GetEndOfScope(lines, linenbr);

					// Go up to parent
					while (linenbr > currentNode.End)
						currentNode = currentNode.Parent;

					// Child
					ClassNode child = new ClassNode(currentNode, start, end, linenbr);
					classNodes.Add(child);
					currentNode.Children.Add(child);
					currentNode = child;
					child.Name = className;
				}
				linenbr = linenbr + skip;
			}

			// Mark all partial nodes
			foreach (ClassNode node in classNodes)
				if (node.Methods.Count != 0) {
					node.Partial = true;
					ClassNode upNode = node;
					do {
						upNode.Parent.Partial = true;
						upNode = upNode.Parent;
					}
					while (upNode != upNode.Parent);
				}

			// Create all directories
			CreateSubDirectories(dest, Path.GetDirectoryName(filename));

			// Create partial file
			if (methodNodes.Count != 0) {
				string partialFile = Path.Combine(dest, filename.Insert(filename.Length - 2, "Partial."));
				Console.WriteLine(partialFile);
				CreatePartialFile(lines, rootNode, usings, namespaces, Path.Combine(dest, partialFile));
			}

			// Modify source file
			CreateModifiedFile(lines, classNodes, methodNodes, Path.Combine(dest, filename));
		}

		static void CreateModifiedFile(string[] lines, List<ClassNode> classNodes, List<MethodNode> methodNodes, string filename)
		{
			// Insert partial
			foreach (ClassNode classNode in classNodes) {
				AddPartialToClassName(ref lines, classNode.Declare);

				foreach (int line in classNode.OtherDeclare)
					AddPartialToClassName(ref lines, line);
			}

			// Insert comments
			foreach (MethodNode method in methodNodes)
				for (int i = method.Start; i <= method.End; i++) {
					string trim = lines[i].Trim(new char[] { '\t', ' ' });

					if ((!string.IsNullOrEmpty(trim))
						&& (!trim.StartsWith("#"))
						&& (!trim.StartsWith("//")))
						lines[i] = @"//" + lines[i];
				}

			// Write modified source files
			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in lines)
					if (line != null)
						writer.WriteLine(line);

				writer.Close();
			}
		}

		static void CreatePartialFile(string[] lines, ClassNode rootNode, List<int> usings, List<int> namespaces, string filename)
		{
			List<string> output = new List<string>();

			// Write "using" lines
			foreach (int i in usings)
				output.Add(lines[i].Trim(new char[] { '\t', ';', ' ' }) + ";");

			output.Add(string.Empty);

			// Write "namespace" lines
			if (namespaces.Count != 1)
				return; // problem, more than one namespace

			output.Add(lines[namespaces[0]].Trim(new char[] { '\t', ';', ' ', '{' }));
			output.Add("{");

			foreach (ClassNode child in rootNode.Children)
				WriteClass(lines, child, output, 0);

			output.Add("}");

			using (TextWriter writer = new StreamWriter(filename)) {
				foreach (string line in output)
					if (line != null)
						writer.WriteLine(line);
				writer.Close();
			}
		}

		static void WriteClass(string[] lines, ClassNode currentNode, List<string> output, int depth)
		{
			if (!currentNode.Partial)
				return;

			string tabs = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t".Substring(0, depth + 1);

			// Write class declaration
			output.Add(tabs + GetDeclaration(lines, currentNode.Declare));
			output.Add(tabs + "{");

			// Write method declarations
			foreach (MethodNode method in currentNode.Methods) {
				string extra = string.Empty;

				for (int i = method.Declare; i <= method.End; i++) {
					string line = lines[i];

					string trim = line.TrimStart(new char[] { '\t', ' ' });

					if ((trim.StartsWith("[")) && (trim.EndsWith("]")))
						continue;

					if (trim.StartsWith("//"))
						continue;

					line = line.Replace(" extern ", " ");
					line = line.Trim(new char[] { '\t', ' ' });

					bool semicolon = line.Contains(";");

					if (semicolon)
						line = line.Replace(";", string.Empty);

					if ((line == "get") || (line == "set"))
						extra = "\t";

					if (!string.IsNullOrEmpty(line)) {
						// Strip internal and extern method attributes
						line = line.Replace(" internal ", " ").Replace(" extern ", " ");

						if (line.StartsWith("internal "))
							line = line.Substring(9);

						if (line.StartsWith("extern "))
							line = line.Substring(7);

						output.Add(tabs + extra + "\t" + line);
					}

					if (semicolon) {
						output.Add(tabs + extra + "\t{");
						output.Add(tabs + extra + "\t\tthrow new System.Exception(\"The method or operation is not implemented.\");");
						output.Add(tabs + extra + "\t}");
						extra = string.Empty;
					}
				}
			}

			output.Add(string.Empty);

			foreach (ClassNode child in currentNode.Children)
				WriteClass(lines, child, output, depth + 1);

			output.Add(tabs + "}");
		}

		static int GetNumberOfMethodDeclarationLines(string[] lines, int at, bool bracket)
		{
			if (lines[at].Contains("{"))
				bracket = true;

			if (bracket)
				if (lines[at].Contains("}"))
					return 0;

			if (!bracket)
				if (lines[at].Contains(";"))
					return 0;

			return 1 + GetNumberOfMethodDeclarationLines(lines, at + 1, bracket);
		}

		static void AddPartialToClassName(ref string[] lines, int line)
		{
			if (line < 0)
				return;

			if (lines[line].Contains(" partial "))
				return;

			int insert = lines[line].IndexOf("class ");

			if (insert < 0)
				insert = lines[line].IndexOf("struct ");

			lines[line] = lines[line].Insert(insert, "partial ");
		}

		static string GetClassName(string[] lines, int line)
		{
			string declare = lines[line].Trim(new char[] { '\t', ' ' });

			// Determine class or struct
			int insert = declare.IndexOf("class ") + 5;

			if (insert < 5)
				insert = declare.IndexOf("struct ") + 6;

			// Get class/struct name
			string name = declare.Substring(insert + 1);
			int end = name.Length;
			int at = name.IndexOf(":");
			if (at > 0) end = at;
			at = name.IndexOf(" ");
			if ((at > 0) && (at < end)) end = at;

			return name.Substring(0, end);
		}

		/// <summary>
		/// Gets the declaration.
		/// </summary>
		/// <param name="lines">The lines.</param>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		static string GetDeclaration(string[] lines, int line)
		{
			string declare = lines[line].Trim(new char[] { '\t', ' ' });

			// Get Name
			string name = GetClassName(lines, line);

			// Determine class or struct
			string obj = (declare.Contains("struct ")) ? "struct" : "class";

			// Determine attribute (public, private, protected)
			string attribute = string.Empty;

			if (declare.IndexOf("public ") >= 0)
				attribute = "public ";
			else if (declare.IndexOf("protected ") >= 0)
				attribute = "protected ";
			else if (declare.IndexOf("private ") >= 0)
				attribute = "private ";

			// Determine if abstract
//			if ((declare.Contains(" abstract ")) || (declare.StartsWith("abstract ")))
//				obj = obj + " abstract";

			return attribute + "partial " + obj + " " + name;
		}

		static void CreateSubDirectories(string root, string directory)
		{
			string current = Path.Combine(root, directory);

			if (Directory.Exists(current))
				return;

			string parent = Path.GetDirectoryName(directory);

			if (!string.IsNullOrEmpty(parent))
				CreateSubDirectories(root, parent);

			Directory.CreateDirectory(current);
		}

		static int GetEndOfScope(string[] lines, int at)
		{
			bool first = false;

			int open = 0;
			int close = 0;

			for (; at < lines.Length; at++) {
				foreach (char c in lines[at])
					if (c == '{')
						open++;
					else if (c == '}')
						close++;

				if ((open > 0) & (!first))
					first = true;

				if ((first) && (open <= close))
					return at;
			}

			return at;
		}

		static int GetPreviousBlockEnd(string[] lines, int at)
		{
			for (at--; at >= 0; at--) {
				foreach (char c in lines[at])
					if ((c == '{') || (c == ';') || (c == '}'))
						return at + 1;
			}

			return 0;
		}

	}
}
