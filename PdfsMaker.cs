using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace TitlePageMaker
{
    public class PdfsMaker
    {
        public string inputFolder;
        public string outputFolder;

        public PdfsMaker(string inputFolder, string outputFolder)
        {
            this.inputFolder = inputFolder;
            this.outputFolder = outputFolder;
        }

        public Task MakePdfs()
        {
            // Создаем пространство выполнения PowerShell
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                using (var pipeline = runspace.CreatePipeline())
                {
                    // Создаем COM-объект Word.Application
                    pipeline.Commands.AddScript(@"
                    $word = New-Object -ComObject Word.Application
                    $word.Visible = $false
                ");

                    // Получаем файлы .docx и конвертируем их в PDF
                    string psScript = $@"
                    $folderPath = '{inputFolder}'
                    $outputFolderPath = '{outputFolder}'
                    
                    $files = Get-ChildItem -Path $folderPath -Filter *.docx

                    foreach ($file in $files) {{
                        $doc = $word.Documents.Open($file.FullName)
                        $pdfFilename = [System.IO.Path]::ChangeExtension($file.Name, '.pdf')
                        $outputPath = Join-Path -Path $outputFolderPath -ChildPath $pdfFilename
                        $doc.SaveAs([String] $outputPath, [ref] 17) # 17 соответствует формату wdFormatPDF
                        $doc.Close()
                    }}

                    $word.Quit()
                    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($word) | Out-Null
                    [System.GC]::Collect()
                    [System.GC]::WaitForPendingFinalizers()
                ";

                    pipeline.Commands.AddScript(psScript);

                    try
                    {
                        // Выполняем скрипт
                        var results = pipeline.Invoke();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }

                runspace.Close();

                return Task.CompletedTask;
            }
        }
    }
}
