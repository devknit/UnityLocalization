
namespace Knit.Localization
{
	public interface ILocalizeListener
	{
		int GetInstanceID();
		int GetLocaleOrder();
		void OnLocaleChanged();
	}
}
