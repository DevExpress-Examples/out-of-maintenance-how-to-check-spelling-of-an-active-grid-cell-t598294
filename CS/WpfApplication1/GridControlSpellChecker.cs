using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.SpellChecker;
using System;
using System.Windows;
using System.Windows.Threading;

namespace WpfApplication1
{
    public class GridControlSpellChecker : DXSpellCheckerBase<GridControl>
    {
        GridControl Grid { get { return AssociatedObject; } }
        protected override void OnAttached()
        {
            base.OnAttached();
            Grid.Dispatcher.BeginInvoke(new Action(() => SubscribeToEvents()), DispatcherPriority.Loaded);
        }
                   
        #region #SubscribeToEvents
        private void SubscribeToEvents()
        {
            SpellChecker.CheckCompleteFormShowing += Checker_CheckCompleteFormShowing;
            CardView cardView = Grid.View as CardView;
            if (cardView != null)
                cardView.ShownEditor += CardView_ShownEditor;
        }
        private void UnsubscribeFromEvents()
        {
            SpellChecker.CheckCompleteFormShowing -= Checker_CheckCompleteFormShowing;
            CardView cardView = Grid.View as CardView;
            if (cardView != null)
                cardView.ShownEditor -= CardView_ShownEditor;
        }
        protected override void OnDetaching()
        {
            UnsubscribeFromEvents();
            base.OnDetaching();
        }
        #endregion #SubscribeToEvents

        #region #ShownEditor
        private void CardView_ShownEditor(object sender, EditorEventArgs e)
        {
            var cardView = (sender as CardView);
            BaseEdit activeEditor = cardView.ActiveEditor;
            if (SpellChecker.SpellCheckMode == DevExpress.XtraSpellChecker.SpellCheckMode.OnDemand)
                CheckActiveEditor(activeEditor);
        }
        void CheckActiveEditor(BaseEdit activeEditor)
        {
            activeEditor.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (SpellChecker.CanCheck(activeEditor))
                    SpellChecker.Check(activeEditor);
            }), DispatcherPriority.Loaded);
        }
        #endregion #ShownEditor

        #region #CompleteFormShowing
        void Checker_CheckCompleteFormShowing(object sender, DevExpress.XtraSpellChecker.FormShowingEventArgs e)
        {
            e.Handled = true;
        }
        #endregion #CompleteFormShowing
    }
}