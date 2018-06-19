using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using HtmlAgilityPack;
using Transitions;
using HtmlDocument = System.Windows.Forms.HtmlDocument;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        HtmlElement currentElement;
        HtmlElement previousElement;
        HtmlDocument thisdoc;
        private string highlightColor = "rgb(255,255,204)";
        private string previousStyle = "";
        private string[] languages = { "Python", "Java" };
        private List<string[]> processedHtml;
        private List<string> htmlStrings;
        private string fullDocumentHtml;
        public Form1()
        {
            //var color = Color.FromArgb();
            InitializeComponent();
            webBrowser1.AllowNavigation = false;
            languageSelection.Items.AddRange( languages );
        }

        private void navigateButton_Click(object sender, EventArgs e)
        {
            webBrowser1.AllowNavigation = true;
            string val = textBox1.Text;
            //Uri val = new Uri( "file:///C:\\Users\\Spencer.Robertson\\Desktop\\test.html" );  
            webBrowser1.Navigate( val );
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            thisdoc = webBrowser1.Document;
            thisdoc.MouseOver += thisDoc_MouseDown;
            thisdoc.MouseDown += SelectDoc;
        }

        void thisDoc_MouseDown( object sender, HtmlElementEventArgs e)
        {
            if ( e.ShiftKeyPressed )
            {
                SelectDoc(sender, e);
            }
        }

        private void SelectDoc(object sender, HtmlElementEventArgs e)
        {
            processedHtml = null;
            htmlStrings = null;
            fullDocumentHtml = null;
            //if (previousElement != null)
            //{
            //    previousElement.Style = previousStyle;
            //}

            webBrowser1.AllowNavigation = false;
            
            HtmlElement currentElement = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);

            string elementStyle = currentElement.Style;
            currentElement.Style = elementStyle + $"; background-color: {highlightColor};";

            currentElement = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
            string elementHtml = currentElement.OuterHtml;

            fullDocumentHtml = webBrowser1.Document.Body.OuterHtml;
            processedHtml = processHtml(elementHtml);

            if ( languageSelection.Text != "Select a language" )
            {
                htmlStrings = checkHtml( fullDocumentHtml, processedHtml, currentElement );

                if ( htmlStrings.Count > 0 )
                {
                    seleniumStrings.Text = htmlStrings.First();
                    seleniumStrings.Items.AddRange( htmlStrings.Skip( 1 ).ToArray() );
                }
            }

            previousElement = currentElement;
            previousStyle = elementStyle;
        }

        private List<string[]> processHtml(string html)
        {
            List<string[]> outputList = new List<string[]>();
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);

            var dom = htmlDocument.DocumentNode.Descendants().First().Name; 
            var element = htmlDocument.DocumentNode.Descendants().First().Attributes.ToList();

            foreach ( var elem in element )
            {
                var elementName = elem.Name;
                var value = elem.Value;
                string[] outputString = { dom, elementName, value};
                outputList.Add(outputString);
            }

            string text = htmlDocument.DocumentNode.Descendants().First().InnerText;
            if ( !String.IsNullOrEmpty(text) )
            {
                string[] domOutput = { dom, text};
                outputList.Add( domOutput );
            }
            return outputList;
        }

        private List<string> checkHtml(string html, List<string[]> outputList, HtmlElement element)
        {
            seleniumStrings.Items.Clear();
            seleniumStrings.Text = "";
            if (previousElement != null)
            {
                previousElement.Style = previousStyle;
            }
            List<string> actualOutput = new List<string>();
            foreach ( var output in outputList)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(html);

                string fullOutput;

                if ( output.Length == 3 )
                { 
                    fullOutput = $"//{output[0]}[@{output[1]}='{output[2]}']";
                }
                else
                {
                    fullOutput = $"//{output[0]}[text()='{output[1]}']";                }

                HtmlNodeCollection nodes = null;
                try
                {
                    nodes = htmlDocument.DocumentNode.SelectNodes(fullOutput);
                }
                catch
                {
                    //Nothing
                }
                try
                {
                    int count = 0;
                    foreach ( var node in nodes )
                    {
                        try
                        {
                            var style = node.Attributes["Style"].Value.Replace( " ", "" );
                            if ( languageSelection.Text == "Python" )
                            {
                                if ( style.Contains( highlightColor ) &&
                                     !fullOutput.Contains( highlightColor ) )
                                {
                                    if ( count == 0 )
                                    {
                                        if ( output[1] == "class" )
                                        {
                                            actualOutput.Add(
                                                "find_element_by_class_name(\"" + output[2] +
                                                "\")" );
                                        }
                                        else
                                        {
                                            actualOutput.Add(
                                                "find_element_by_xpath(\"" + fullOutput + "\")" );
                                        }
                                    }
                                    else
                                    {
                                        if ( output[1] == "class" )
                                        {
                                            actualOutput.Add(
                                                "find_elements_by_class_name(\"" + output[2] +
                                                "\")[" +
                                                count + "]" );
                                        }
                                        else
                                        {
                                            actualOutput.Add(
                                                "find_elements_by_xpath(\"" + fullOutput + "\")[" +
                                                count +
                                                "]" );
                                        }
                                    }

                                }
                            }

                            if (languageSelection.Text == "Java")
                            {
                                if (style.Contains(highlightColor) &&
                                     !fullOutput.Contains(highlightColor))
                                {
                                    if (count == 0)
                                    {
                                        actualOutput.Add(
                                            "findElement(By.xpath(\"" + fullOutput + "\")");
                                    }
                                    else
                                    {
                                        actualOutput.Add(
                                            "findElements(By.xpath(\"" + fullOutput + "\")[" +
                                            count +
                                            "]");
                                    }

                                }
                            }
                        }
                        catch
                        {
                            //Nothing
                        }
                        count++;
                    }
                }
                catch
                {
                    //Nothing
                }
            }
            return actualOutput;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                navigateButton_Click(this, new EventArgs());
            }
        }

        private void languageSelection_TextChanged_1(object sender, EventArgs e)
        {
            if (languageSelection.Text != "Select a language")
            {
                try
                {
                    htmlStrings = checkHtml( fullDocumentHtml, processedHtml, currentElement );

                    if ( htmlStrings.Count > 0 )
                    {
                        seleniumStrings.Text = htmlStrings.First();
                        seleniumStrings.Items.AddRange( htmlStrings.Skip( 1 ).ToArray() );
                    }
                }
                catch
                {
                    //nothing
                }
            }
        }

        private int count = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if ( count == 0 )
            {
                Console.Write( "BUTTON CLICKED" );
                var t = new Transition( new TransitionType_EaseInEaseOut( 500 ) );
                t.add( seleniumStrings, "Left", -1000 );
                t.add( seleniumStrings, "Top", seleniumStrings.Top );

                t.add( languageSelection, "Left", -1000 );
                t.add( languageSelection, "Top", languageSelection.Top );

                t.add( navigateButton, "Left", -1000 );
                t.add( navigateButton, "Top", navigateButton.Top );

                t.add( textBox1, "Left", -1000 );
                t.add( textBox1, "Top", textBox1.Top );

                t.add( webBrowser1, "Left", -1000 );
                t.add( webBrowser1, "Top", webBrowser1.Top );

                t.add( button1, "Left", navigateButton.Left );
                t.add( button1, "Top", button1.Top );
                t.run();
                count = 1;
            }
            else
            {
                var t = new Transition(new TransitionType_EaseInEaseOut(500));
                t.add(seleniumStrings, "Left", 13);
                t.add(seleniumStrings, "Top", seleniumStrings.Top);

                t.add(languageSelection, "Left", 573);
                t.add(languageSelection, "Top", languageSelection.Top);

                t.add(navigateButton, "Left", 12);
                t.add(navigateButton, "Top", navigateButton.Top);

                t.add(textBox1, "Left", 94);
                t.add(textBox1, "Top", textBox1.Top);

                t.add(webBrowser1, "Left", 12);
                t.add(webBrowser1, "Top", webBrowser1.Top);

                t.add(button1, "Left", 655);
                t.add(button1, "Top", button1.Top);
                t.run();
                count = 0;
            }
            
        }
    }
}
