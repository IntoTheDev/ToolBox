using Sirenix.OdinInspector;
using System.IO;

namespace ToolBox.Editor
{
	public partial class ToolBoxMenu
	{
		public class TweenGenerator : ScriptGenerationWindow
		{
			[Button("Generate Tweener", ButtonSizes.Medium)]
			protected override void GenerateScript()
			{
				SetData();

				_scriptName = $"Tween{_scriptName}";
				string path = GenerateFile(_folder, _scriptName, _template);

				string fileContent = File.ReadAllText(path);
				fileContent = ReplaceText(path, fileContent);
				File.WriteAllText(path, fileContent);

				SelectFile(path);
			}

			protected override string ReplaceText(string path, string fileContent)
			{
				fileContent = ReplaceContent(path, fileContent, "#SCRIPTNAME#", _scriptName);
				return fileContent;
			}

			protected override void SetData()
			{
				_template = "Assets/[1]ToolBox/Core/Editor/Templates/TweenerTemplate.cs.txt";
				_folder = "Assets/[1]ToolBox/Reactors/Generated/Tweeners";
			}
		}
	}
}
