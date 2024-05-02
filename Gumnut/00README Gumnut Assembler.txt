
Open a "CMD" window with administrator privileges

Go to the GumnutAssembler directory, e.g. "d:/GumnutAssembler"

if you type "DIR", you should see the followint files:

	00README.txt	
	antlr.jar
	antlr.zip
	example.gsm
	example1.gsm
	Gasm.jar
	Gasm.zip
	gasm-manual v0.pdf
	gasm-manual v1.pdf

In order to assemble a program, you have to set up CLASSPATH by typing

>set CLASSPATH=d:/GumnutAssembler/Gasm.jar;d:/GumnutAssembler/antlr.jar

To assemble a program, type
>java Gasm example.gsm


	
