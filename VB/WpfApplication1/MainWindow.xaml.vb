Imports DevExpress.Xpf.Editors
Imports DevExpress.Xpf.Grid
Imports DevExpress.Xpf.SpellChecker
Imports System
Imports System.Windows

Namespace WpfApplication1
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            DataContext = New ViewModel()
            InitializeComponent()
        End Sub


    End Class
End Namespace