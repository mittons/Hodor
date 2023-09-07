using Newtonsoft.Json;

namespace Contracts.Dto
{
    public class HodorTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ParentId { get; set; }
        public HodorTaskStatus CurrentTaskStatus { get; set; }
        public int Ordinal { get; set; }

        public override bool Equals(object obj)
        {
            var task = obj as HodorTask;

            if (task == null || task.Title != Title || task.CurrentTaskStatus != CurrentTaskStatus ||  task.ParentId != ParentId || task.Ordinal != Ordinal) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public HodorTask Clone()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<HodorTask>(serialized);
        }

        public const int PROJECT_TREE_ROOT_PARENT_ID = -1;

    }

    public enum HodorTaskStatus
    {
        Todo,
        Impending,
        Completed
    }
}
