using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using HtmlAgilityPack;
using MetroFramework.Forms;
using Transitions;
using HtmlDocument = System.Windows.Forms.HtmlDocument;

namespace WindowsFormsApplication1
{
    public partial class Form1 : MetroForm
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

        private void CreateMyListView(List<string> htmlStrings)
        {

            // Set the view to show details.
            listView1.View = View.Details;
            // Allow the user to edit item text.
            listView1.LabelEdit = true;
            listView1.LabelWrap = true;
            // Allow the user to rearrange columns.
            listView1.AllowColumnReorder = true;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;

            // Create columns for the items and subitems.
            listView1.Columns.Add("Column 1", 0, HorizontalAlignment.Left);
            listView1.Columns.Add(languageSelection.Text, 400, HorizontalAlignment.Left);

            foreach (var htmlString in htmlStrings)
            {
                ListViewItem listItem1 = new ListViewItem("");
                listItem1.SubItems.Add(new ListViewItem.ListViewSubItem(
                    listItem1, htmlString));
                listView1.Items.Add(listItem1);

            }


        }

        public Form1()
        {
            //var color = Color.FromArgb();
            InitializeComponent();
            webBrowser1.AllowNavigation = false;
            languageSelection.Items.AddRange( languages );
            richTextBox1.Location = new Point(this.Width + 2000, richTextBox1.Top);
            //CreateMyListView();
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
            if (previousElement != null)
            {
                previousElement.Style = previousStyle;
            }

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
                htmlStrings = checkHtml( fullDocumentHtml, processedHtml );

                if ( htmlStrings.Count > 0 )
                {
                    CreateMyListView(htmlStrings);
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

        private List<string> checkHtml(string html, List<string[]> outputList)
        {
            listView1.Columns.Clear();
            listView1.Items.Clear();
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
                    htmlStrings = checkHtml( fullDocumentHtml, processedHtml );

                    if ( htmlStrings.Count > 0 )
                    {
                        CreateMyListView(htmlStrings);
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
            Dictionary<string, int> defaultPositions = new Dictionary<string, int>()
            {
                {"languageSelection", this.Width - languageSelection.Width - 30},
                {"navigateButton", 12},
                {"textBox1", 94},
                {"webBrowser1", 12},
                {"button1", this.Width - button1.Width - 30 },
                {"listView1", this.Width - listView1.Width - 30 },
                {"richTextBox1", this.Width  + 2000}
            };

            Dictionary<string, int> secondaryPositions = new Dictionary<string, int>()
            {
                {"languageSelection", navigateButton.Left},
                {"navigateButton", -navigateButton.Width - 12- 2000},
                {"textBox1", -textBox1.Width - 194 - 2000},
                {"webBrowser1", -webBrowser1.Width - 12- 2000},
                {"button1", navigateButton.Left },
                {"listView1", navigateButton.Left },
                {"richTextBox1", navigateButton.Left + button1.Width + 15}
            };

            Dictionary<string, AnchorStyles> defaultAnchors = new Dictionary<string, AnchorStyles>()
            {
                {"languageSelection", AnchorStyles.Right | AnchorStyles.Bottom},
                {"button1", AnchorStyles.Right | AnchorStyles.Top },
                {"listView1", AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom },
                //{"richTextBox1", AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right}
            };

            Dictionary<string, AnchorStyles> secondaryAnchors = new Dictionary<string, AnchorStyles>()
            {
                {"languageSelection", AnchorStyles.Left | AnchorStyles.Bottom},
                {"button1", AnchorStyles.Left | AnchorStyles.Top },
                {"listView1", AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom },
                //{"richTextBox1", AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom }
            };

            if ( count == 0 )
            {
                transitionViews( secondaryPositions );
                count = 1;
                setAnchors(secondaryAnchors);
            }
            else
            {
                transitionViews(defaultPositions);
                count = 0;
                setAnchors(defaultAnchors);
            }
        }

        private void setAnchors( Dictionary<string, AnchorStyles> anchors)
        {
            languageSelection.Anchor = anchors["languageSelection"];
            button1.Anchor = anchors["button1"];
            listView1.Anchor = anchors["listView1"];
            //richTextBox1.Anchor = anchors["richTextBox1"];
        }

        private void transitionViews(Dictionary<string, int> positions)
        {
            var t = new Transition(new TransitionType_EaseInEaseOut(500));
            t.add(languageSelection, "Left", positions["languageSelection"]);
            t.add(languageSelection, "Top", languageSelection.Top);

            t.add(navigateButton, "Left", positions["navigateButton"]);
            t.add(navigateButton, "Top", navigateButton.Top);

            t.add(textBox1, "Left", positions["textBox1"]);
            t.add(textBox1, "Top", textBox1.Top);

            t.add(webBrowser1, "Left", positions["webBrowser1"]);
            t.add(webBrowser1, "Top", webBrowser1.Top);

            t.add(button1, "Left", positions["button1"]);
            t.add(button1, "Top", button1.Top);

            t.add(listView1, "Left", positions["listView1"]);
            t.add(listView1, "Top", listView1.Top);

            t.add(richTextBox1, "Left", positions["richTextBox1"]);
            t.add(richTextBox1, "Top", richTextBox1.Top);
            t.run();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point mousePos = listView1.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hitTest = listView1.HitTest(mousePos);
            int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
            richTextBox1.AppendText( hitTest.Item.SubItems[columnIndex].Text + Environment.NewLine);
        }
    }
}
