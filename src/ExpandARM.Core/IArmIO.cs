namespace ExpandARM.Core
{
    public interface IArmIO
    {
        ArmTemplate LoadArmTemplate(string filePath);

        void ExpandArmTemplate(ArmTemplate armTemplate);

        void SaveExpandedTemplate(ArmTemplate armTemplate);
    }
}
