﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LeetCodeClone
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Press Enter key add a new line to code textbox
        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                TextBox textBox = (TextBox)sender;
                int caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, Environment.NewLine);
                textBox.CaretIndex = caretIndex + Environment.NewLine.Length;
            }
        }


        //Allow to paste multyline code in code textbox
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

                if (!string.IsNullOrEmpty(pastedText))
                {
                    TextBox textBox = (TextBox)sender;
                    int caretIndex = textBox.CaretIndex;

                    if (caretIndex >= 0 && caretIndex <= textBox.Text.Length)
                    {
                        string newText = textBox.Text.Insert(caretIndex, pastedText);
                        textBox.Text = newText;

                        textBox.CaretIndex = caretIndex + pastedText.Length;
                    }
                }
            }
            e.CancelCommand();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}