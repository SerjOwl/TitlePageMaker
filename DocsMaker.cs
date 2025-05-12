using NameCaseLib;
using NetOffice.WordApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitlePageMaker
{
    public class DocsMaker
    {
        public string outputFolder;
        public string templateFile;
        public string[] keys;
        public List<string[]> values;


        public DocsMaker(string outputFolder, string templateFile)
        {
            this.outputFolder = outputFolder;
            this.templateFile = templateFile;
        }

        private string GetFioShort(string fio)
        {
            string[] fioSplit = fio.Split(' ').ToArray();

            return $"{fioSplit[0]} {fioSplit[1][0]}.{fioSplit[2][0]}.";
        }

        public Task MakeDocs()
        {
            var ncl = new Ru();

            foreach (string[] value in values)
            {
                string ResultFileName = outputFolder + @"\" + GetFioShort(value[0]) + ".docx";
                var wordApp = new NetOffice.WordApi.Application();
                var doc = wordApp.Documents.Open(this.templateFile, false, true);

                for (int i = 0; i < keys.Length; i++)
                {
                    doc.Content.Find.Execute(findText: keys[i],
                                             matchCase: false,
                                             matchWholeWord: false,
                                             matchWildcards: false,
                                             matchSoundsLike: false,
                                             matchAllWordForms: false,
                                             forward: true,
                                             wrap: false,
                                             format: false,
                                             replaceWith: value[i],
                                             replace: WdReplace.wdReplaceAll
                                             );
                }

                int index = Array.IndexOf(keys, "{FIO}");

                if (index != -1)
                {
                    doc.Content.Find.Execute(findText: "{FIO1}",
                                             matchCase: false,
                                             matchWholeWord: false,
                                             matchWildcards: false,
                                             matchSoundsLike: false,
                                             matchAllWordForms: false,
                                             forward: true,
                                             wrap: false,
                                             format: false,
                                             replaceWith: ncl.QFullName(value[index])[2],
                                             replace: WdReplace.wdReplaceAll
                                             );

                    doc.Content.Find.Execute(findText: "{FIO2}",
                                             matchCase: false,
                                             matchWholeWord: false,
                                             matchWildcards: false,
                                             matchSoundsLike: false,
                                             matchAllWordForms: false,
                                             forward: true,
                                             wrap: false,
                                             format: false,
                                             replaceWith: GetFioShort(value[index]),
                                             replace: WdReplace.wdReplaceAll
                                             );
                }

                doc.SaveAs(ResultFileName);
                doc.Close();

            }

            return Task.CompletedTask;
        }
        public Task GetData(string path)
        {
            List<string[]> strings = new List<string[]>();

            //StreamReader streamReader = new StreamReader(@"C:\words\New folder\template.csv");
            StreamReader streamReader = new StreamReader(path);
            string line = streamReader.ReadLine();

            this.keys = line.Split(",").ToArray();

            line = streamReader.ReadLine();
            while (line != null)
            {
                string[] temp = line.Split(",").ToArray();

                strings.Add(temp);

                line = streamReader.ReadLine();
            }

            this.values = strings;

            return Task.CompletedTask;
        }
    }
}
