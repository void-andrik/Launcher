using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace avLauncher
{
    class Program
    {
        static string getFileVersion(string fileName)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
            return fvi.FileVersion;
        }

        static void findCopyAndExecute()
        {
            string runName = "";
            string workDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string exeName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string origName = string.Format("{0}.orig", exeName);
            try
            {
                string origVersion = getFileVersion(string.Format(@"{0}\{1}", workDirectory, origName));
                string[] currentFileList = Directory.GetFiles(@workDirectory, exeName + "_current*.exe");
                foreach (string fileName in currentFileList)
                {
                    string checkVersion = getFileVersion(fileName);
                    if (checkVersion == origVersion) runName = fileName;
                    else
                    {
                        try { File.Delete(fileName); }
                        catch { }
                    }
                }
                if (string.IsNullOrEmpty(runName))
                {
                    int counter = -1;
                    while (true)
                    {
                        counter++;
                        string checkName = string.Format("{0}_current{1}.exe", exeName, counter);
                        if (!currentFileList.Contains(checkName))
                        {
                            try
                            {
                                runName = checkName;
                                File.Copy(workDirectory + @"\" + origName, checkName);
                            }
                            catch { }
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(runName))
                {
                    var appPrc = new Process { StartInfo = new ProcessStartInfo { FileName = runName } };
                    appPrc.Start();
                }
            }
            catch {
                MessageBox.Show("Ошибка: Связанный файл не найден");
            }
        }

        public static void CompileExecutable(string compileParams = null)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            String exeName = String.Format(@"{0}\Launcher_{1}.exe", System.Environment.CurrentDirectory, DateTime.Now.ToLongTimeString().Replace(":", "_"));
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = true;
            cp.OutputAssembly = exeName;
            cp.GenerateInMemory = false;
            cp.TreatWarningsAsErrors = false;
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("mscorlib.dll");
            if (!string.IsNullOrEmpty(compileParams))
                cp.CompilerOptions = compileParams;
            string src = ReplaceLastOccurrence(srcCode, "@CODE@", srcCode.Replace("\"", "\"\""));
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, src);
        }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.LastIndexOf(Find);
            return Source.Remove(Place, Find.Length).Insert(Place, Replace);
        }  

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Count() == 0) findCopyAndExecute();
            else
            {
                string ico = args[0];
                string icoParam = string.Format(@"/target:winexe /win32icon:{0}", ico);
                CompileExecutable(icoParam);
            }
        }
        static string srcCode = @"
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace avLauncher
{
    class Program
    {
        static string getFileVersion(string fileName)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
            return fvi.FileVersion;
        }
        static void findCopyAndExecute()
        {
            string runName = """";
            string workDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string exeName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string origName = string.Format(""{0}.orig"", exeName);
            try
            {
                string origVersion = getFileVersion(string.Format(@""{0}\{1}"", workDirectory, origName));
                string[] currentFileList = Directory.GetFiles(@workDirectory, exeName + ""_current*.exe"");
                foreach (string fileName in currentFileList)
                {
                    string checkVersion = getFileVersion(fileName);
                    if (checkVersion == origVersion) runName = fileName;
                    else
                    {
                        try { File.Delete(fileName); }
                        catch { }
                    }
                }
                if (string.IsNullOrEmpty(runName))
                {
                    int counter = -1;
                    while (true)
                    {
                        counter++;
                        string checkName = string.Format(""{0}_current{1}.exe"", exeName, counter);
                        if (!currentFileList.Contains(checkName))
                        {
                            try
                            {
                                runName = checkName;
                                File.Copy(workDirectory + @""\"" + origName, checkName);
                            }
                            catch { }
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(runName))
                {
                    var appPrc = new Process { StartInfo = new ProcessStartInfo { FileName = runName } };
                    appPrc.Start();
                }
            }
            catch {
                MessageBox.Show(""Ошибка: Связанный файл не найден"");
            }
        }
        public static void CompileExecutable(string compileParams = null)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider(""CSharp"");
            String exeName = String.Format(@""{0}\Launcher_{1}.exe"", System.Environment.CurrentDirectory, DateTime.Now.ToLongTimeString().Replace("":"", ""_""));
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = true;
            cp.OutputAssembly = exeName;
            cp.GenerateInMemory = false;
            cp.TreatWarningsAsErrors = false;
            cp.ReferencedAssemblies.Add(""System.dll"");
            cp.ReferencedAssemblies.Add(""System.Core.dll"");
            cp.ReferencedAssemblies.Add(""System.Windows.Forms.dll"");
            cp.ReferencedAssemblies.Add(""mscorlib.dll"");
            if (!string.IsNullOrEmpty(compileParams))
                cp.CompilerOptions = compileParams;
            string src = ReplaceLastOccurrence(srcCode, ""@CODE@"", srcCode.Replace(""\"""", ""\""\""""));
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, src);
        }
        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.LastIndexOf(Find);
            return Source.Remove(Place, Find.Length).Insert(Place, Replace);
        }  
        static void Main(string[] args)
        {
            if (args.Count() == 0) findCopyAndExecute();
            else
            {
                string ico = args[0];
                string icoParam = string.Format(@""/target:winexe /win32icon:{0}"", ico);
                CompileExecutable(icoParam);
            }
        }
        static string srcCode = @""
@CODE@		
"";
    }
}";
    }
}
