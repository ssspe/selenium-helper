using System;
using System.Collections.Generic;
using System.Data;
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
            seleniumList.View = View.Details;
            // Allow the user to edit item text.
            seleniumList.LabelEdit = true;
            //seleniumList.LabelWrap = true;
            // Allow the user to rearrange columns.
            seleniumList.AllowColumnReorder = true;
            // Select the item and subitems when selection is made.
            seleniumList.FullRowSelect = true;

            // Create columns for the items and subitems.
            seleniumList.Columns.Add(languageSelection.Text, 300, HorizontalAlignment.Left);

            foreach (var htmlString in htmlStrings)
            {
                ListViewItem listItem1 = new ListViewItem(htmlString);
                seleniumList.Items.Add(listItem1);
            }


        }

        public Form1()
        {
            InitializeComponent();
            languageSelection.Items.AddRange( languages );
            richTextBox1.Location = new Point(this.Width + 2000, richTextBox1.Top);
        }

        private void navigate(object sender, EventArgs e)
        {
            string val = navigationBar.Text;
            //Uri val = new Uri( "file:///C:\\Users\\Spencer.Robertson\\Desktop\\test.html" );  
            webBrowser1.Navigate( val );
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            navigateToggle.Checked = true;
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
            seleniumList.Columns.Clear();
            seleniumList.Items.Clear();
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

        private void navigationBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                navigate(this, new EventArgs());
            }
        }

        private int count = 0;
        private bool setLength = true;
        private void textEditorButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> defaultPositions = new Dictionary<string, int>()
            {
                {"languageSelection", this.Width - languageSelection.Width - 30},
                {"navigationBar", 24 + navigateToggle.Width},
                {"webBrowser1", 12},
                {"textEditorButton", this.Width - textEditorButton.Width - 30 },
                {"seleniumList", this.Width - seleniumList.Width - 30 },
                {"richTextBox1", this.Width  + 2000},
                {"navigateToggle", 12}
            };

            Dictionary<string, int> secondaryPositions = new Dictionary<string, int>()
            {
                {"languageSelection", webBrowser1.Left},
                {"navigationBar", -navigationBar.Width - 194 - 2000},
                {"webBrowser1", -webBrowser1.Width - 12- 2000},
                {"textEditorButton", webBrowser1.Left },
                {"seleniumList", webBrowser1.Left },
                {"richTextBox1", webBrowser1.Left + textEditorButton.Width + 15},
                {"navigateToggle", -navigateToggle.Width - 12- 2000}
            };

            Dictionary<string, AnchorStyles> defaultAnchors = new Dictionary<string, AnchorStyles>()
            {
                {"languageSelection", AnchorStyles.Right | AnchorStyles.Bottom},
                {"textEditorButton", AnchorStyles.Right | AnchorStyles.Top },
                {"seleniumList", AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom },
            };

            Dictionary<string, AnchorStyles> secondaryAnchors = new Dictionary<string, AnchorStyles>()
            {
                {"languageSelection", AnchorStyles.Left | AnchorStyles.Bottom},
                {"textEditorButton", AnchorStyles.Left | AnchorStyles.Top },
                {"seleniumList", AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom },
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
                if ( setLength )
                {
                    navigationBar.Width = navigationBar.Width - 20;
                    webBrowser1.Width = webBrowser1.Width - 20;
                }
                setLength = false;
            }
        }

        private void setAnchors( Dictionary<string, AnchorStyles> anchors)
        {
            languageSelection.Anchor = anchors["languageSelection"];
            textEditorButton.Anchor = anchors["textEditorButton"];
            seleniumList.Anchor = anchors["seleniumList"];
        }

        private void transitionViews(Dictionary<string, int> positions)
        {
            var t = new Transition(new TransitionType_EaseInEaseOut(500));
            t.add(languageSelection, "Left", positions["languageSelection"]);
            t.add(languageSelection, "Top", languageSelection.Top);

            t.add(navigationBar, "Left", positions["navigationBar"]);
            t.add(navigationBar, "Top", navigationBar.Top);

            t.add(webBrowser1, "Left", positions["webBrowser1"]);
            t.add(webBrowser1, "Top", webBrowser1.Top);

            t.add(textEditorButton, "Left", positions["textEditorButton"]);
            t.add(textEditorButton, "Top", textEditorButton.Top);

            t.add(seleniumList, "Left", positions["seleniumList"]);
            t.add(seleniumList, "Top", seleniumList.Top);

            t.add(richTextBox1, "Left", positions["richTextBox1"]);
            t.add(richTextBox1, "Top", richTextBox1.Top);

            t.add(navigateToggle, "Left", positions["navigateToggle"]);
            t.add(navigateToggle, "Top", navigateToggle.Top);
            t.run();
        }

        private void seleniumList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point mousePos = seleniumList.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hitTest = seleniumList.HitTest(mousePos);
            int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
            richTextBox1.AppendText( hitTest.Item.SubItems[columnIndex].Text + Environment.NewLine);
        }

        private void languageSelection_TextChanged(object sender, EventArgs e)
        {
            if (languageSelection.Text != "Select a language")
            {
                try
                {
                    htmlStrings = checkHtml(fullDocumentHtml, processedHtml);

                    if (htmlStrings.Count > 0)
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

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            navigationBar.Visible = !navigationBar.Visible;
            webBrowser1.AllowNavigation = !webBrowser1.AllowNavigation;
        }
    }
}
