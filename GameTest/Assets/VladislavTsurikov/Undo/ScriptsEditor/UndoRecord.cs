#if UNITY_EDITOR
namespace VladislavTsurikov.Undo.ScriptsEditor
{
    public abstract class UndoRecord
    {
        public abstract void Undo();
        public abstract void Merge(UndoRecord record);
    }
}
#endif