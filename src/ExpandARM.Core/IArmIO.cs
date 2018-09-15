namespace ExpandARM.Core
{
    public interface IArmIO
    {
        ArmTemplate LoadArmTemplate(string filePath);

        void ExpandArmTemplate(ArmTemplate armTemplate);

        string SaveExpandedTemplate(ArmTemplate armTemplate);

        string SaveExpandedTemplate(ArmTemplate armTemplate, string outputFilePath);
    }
}
