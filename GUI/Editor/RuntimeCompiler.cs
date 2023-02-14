using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
namespace ArchEngine.GUI.Editor
{
    public class RuntimeCompiler<T> where T : class
{
    public List<Type> types = new List<Type>();
    private List<T> typeObjects = new List<T>();

    private AssemblyLoadContext context;
    private WeakReference contextWeak;

    public int errorsCount = 0;

    public List<T> GetObjects()
    {
        return typeObjects;
    }

    public void UnLoad()
    {
        types.Clear();
        typeObjects.Clear();
        context?.Unload();
        context = null;
        if (contextWeak == null)
        {
            return;
        }
        for (var i = 0; i < 8 && contextWeak.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        Console.WriteLine(contextWeak.IsAlive ? "Unloading failed (for now)!" : "Unloading success!");
    }

    public void Load()
    {
        types.ForEach(type => { typeObjects.Add(Activator.CreateInstance(type) as T);});
        Console.WriteLine(typeObjects.Count + " Objects are loaded.");
    }
   
    public void Compile(string scriptFolder)
    {
        errorsCount = 0;
        Console.WriteLine("Compiling scripts..");
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        // Load the scripts from the folder
        string[] scriptFiles = Directory.GetFiles(scriptFolder, "*.cs", SearchOption.AllDirectories);
        string[] scripts = new string[scriptFiles.Length];
        for (int i = 0; i < scriptFiles.Length; i++)
        {

            scripts[i] = "//" + scriptFiles[i] + "\n" + File.ReadAllText(scriptFiles[i]);

        }

        // Compile the scripts into an assembly
        SyntaxTree[] syntaxTrees = new SyntaxTree[scripts.Length];
        for (int i = 0; i < scripts.Length; i++)
        {
            syntaxTrees[i] = CSharpSyntaxTree.ParseText(scripts[i]);
        }
        
        //string currentNamespaceAssemblyLocation = Assembly.GetExecutingAssembly().Location;
        
        var assembly2 = Assembly.GetExecutingAssembly();
        var path = assembly2.CodeBase;
        var uri = new UriBuilder(path).Uri;
        //Console.WriteLine("path:" + uri.LocalPath);
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
            MetadataReference.CreateFromFile(uri.LocalPath)
        };

        Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList().ForEach(a =>
        {
            var path = Assembly.Load(a).Location;
            if (!string.IsNullOrEmpty(path))
            {
                references.Add(MetadataReference.CreateFromFile(path));
            }
        });
        CSharpCompilation compilation = CSharpCompilation.Create("CompiledAssembly", syntaxTrees, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        context = new AssemblyLoadContext("",true);

        using (var ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                Console.WriteLine("Compilation failed with errors:");
                foreach (var diagnostic in result.Diagnostics)
                {
                    errorsCount++;
                    var source = diagnostic.Location.SourceTree.ToString();
                    var fileName = new string(source.TakeWhile(c => c != '\n').ToArray()).Substring(2);
                    var line = diagnostic.Location.GetLineSpan().StartLinePosition;
                    Console.WriteLine($"{fileName}({line.Line},{line.Character}): {diagnostic.Severity.ToString()} {diagnostic.Id}: {diagnostic.GetMessage()}");
                    //Console.WriteLine(diagnostic.Location.SourceTree.GetLineSpan() + diagnostic.ToString());
                }
                return;
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = context.LoadFromStream(ms);

                
                
                stopWatch.Stop();
                Console.WriteLine("Loading Took ms: " + stopWatch.ElapsedMilliseconds);
            
                foreach (var scriptFile in scriptFiles)
                {
                    string typeName = Path.GetFileNameWithoutExtension(scriptFile);
                    //object instance = assembly.CreateInstance(typeName);
                    Type classType = assembly.GetType(typeName);
                    if (classType != null)
                    {
                        if (typeof(T).IsAssignableFrom(classType))
                        {
                            types.Add(classType);
                            //Console.WriteLine(scriptFile + " inherits");
                        }
                    
                    }
                    else
                    {
                        Console.WriteLine(scriptFile + " doesn't have any class with same name.");
                    }
                }

            }
        }
        contextWeak = new WeakReference(context);

    }

}
}