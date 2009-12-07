mkdir build
rm ./build/kernel.exe
mono ../../Mosa/Bin/mosacl.exe -a=x86 -f=PE -pe-file-alignment=4096 -b=mb0.7 -o ./build/kernel.exe ../../Mosa/Bin/Mosa.HelloWorld.exe
