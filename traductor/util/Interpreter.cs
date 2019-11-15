using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace traductor.util
{
    class Interpreter
    {
        public Interpreter()
        {
        }

        public string CompileAndRun(string code, string className)
        {
            CompilerParameters CompilerParams = new CompilerParameters();
            string outputDirectory = Directory.GetCurrentDirectory();

            CompilerParams.GenerateInMemory = true;
            CompilerParams.TreatWarningsAsErrors = false;
            CompilerParams.GenerateExecutable = false;
            CompilerParams.CompilerOptions = "/optimize";

            string[] references = { "System.dll" };
            CompilerParams.ReferencedAssemblies.AddRange(references);

            StringBuilder sb = new StringBuilder("");
            sb.Append("using System;\n");
            sb.Append("using System.Collections.Generic;\n");
            sb.Append("using System.Text;\n");
            sb.Append("using System.Threading.Tasks;\n");

            sb.Append("namespace traductor{ \n");
            sb.Append("}\n");
            sb.Append(code);
            sb.Replace("static", "static public");
            sb.Replace("string[] args", "");
            // Console.WriteLine(sb);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, sb.ToString());

            if (compile.Errors.HasErrors)
            {
                string text = "Compile error: ";
                foreach (CompilerError ce in compile.Errors)
                {
                    text += "rn" + ce.ToString();
                }
                throw new Exception(text);
            }

            // ExpoloreAssembly(compile.CompiledAssembly);

            Module module = compile.CompiledAssembly.GetModules()[0];
            Type mt = null;
            MethodInfo methInfo = null;

            if (module != null)
            {
                mt = module.GetType(className);
            }

            if (mt != null)
            {
                methInfo = mt.GetMethod("Main");
            }

            if (methInfo != null)
            {
                StringWriter stringWriter = new StringWriter();
                Console.SetOut(stringWriter);

                Console.WriteLine(methInfo.Invoke(null, null));
                string consoleOutput = stringWriter.ToString();

                return consoleOutput;
            }

            return "";
        }

        public void ExpoloreAssembly(Assembly assembly)
        {
            Console.WriteLine("Modules in the assembly:");
            foreach (Module m in assembly.GetModules())
            {
                Console.WriteLine("{0}", m);

                foreach (Type t in m.GetTypes())
                {
                    Console.WriteLine("t{0}", t.Name);

                    foreach (MethodInfo mi in t.GetMethods())
                    {
                        Console.WriteLine("tt{0}", mi.Name);
                    }
                }
            }
        }
    }
}
