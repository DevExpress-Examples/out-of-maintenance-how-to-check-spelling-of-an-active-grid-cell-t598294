Imports DevExpress.Xpf.Editors
Imports DevExpress.Xpf.Grid
Imports DevExpress.Xpf.SpellChecker
Imports System
Imports System.Windows
Imports System.Windows.Threading

Namespace WpfApplication1
	Public Class GridControlSpellChecker
		Inherits DXSpellCheckerBase(Of GridControl)

		Private ReadOnly Property Grid() As GridControl
			Get
				Return AssociatedObject
			End Get
		End Property
		Protected Overrides Sub OnAttached()
			MyBase.OnAttached()
			Grid.Dispatcher.BeginInvoke(New Action(Sub() SubscribeToEvents()), DispatcherPriority.Loaded)
		End Sub

		#Region "#SubscribeToEvents"
		Private Sub SubscribeToEvents()
			AddHandler SpellChecker.CheckCompleteFormShowing, AddressOf Checker_CheckCompleteFormShowing
			Dim cardView As CardView = TryCast(Grid.View, CardView)
			If cardView IsNot Nothing Then
				AddHandler cardView.ShownEditor, AddressOf CardView_ShownEditor
			End If
		End Sub
		Private Sub UnsubscribeFromEvents()
			RemoveHandler SpellChecker.CheckCompleteFormShowing, AddressOf Checker_CheckCompleteFormShowing
			Dim cardView As CardView = TryCast(Grid.View, CardView)
			If cardView IsNot Nothing Then
				RemoveHandler cardView.ShownEditor, AddressOf CardView_ShownEditor
			End If
		End Sub
		Protected Overrides Sub OnDetaching()
			UnsubscribeFromEvents()
			MyBase.OnDetaching()
		End Sub
		#End Region ' #SubscribeToEvents

		#Region "#ShownEditor"
		Private Sub CardView_ShownEditor(ByVal sender As Object, ByVal e As EditorEventArgs)
			Dim cardView As CardView = (TryCast(sender, CardView))
			Dim activeEditor As BaseEdit = cardView.ActiveEditor
			If SpellChecker.SpellCheckMode = DevExpress.XtraSpellChecker.SpellCheckMode.OnDemand Then
				CheckActiveEditor(activeEditor)
			End If
		End Sub
		Private Sub CheckActiveEditor(ByVal activeEditor As BaseEdit)
			activeEditor.Dispatcher.BeginInvoke(New Action(Sub()
				If SpellChecker.CanCheck(activeEditor) Then
					SpellChecker.Check(activeEditor)
				End If
			End Sub), DispatcherPriority.Loaded)
		End Sub
		#End Region ' #ShownEditor

		#Region "#CompleteFormShowing"
		Private Sub Checker_CheckCompleteFormShowing(ByVal sender As Object, ByVal e As DevExpress.XtraSpellChecker.FormShowingEventArgs)
			e.Handled = True
		End Sub
		#End Region ' #CompleteFormShowing
	End Class
End Namespace