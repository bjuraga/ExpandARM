namespace MergeARM.Core
{
    public interface IArmIO
    {
        string WorkingDirectory { get; }

        ArmTemplate LoadArmTemplate(string filePath);

        void ExpandArmTemplate(ArmTemplate armTemplate);

        void SaveExpandedTemplate(ArmTemplate armTemplate);
    }
}
