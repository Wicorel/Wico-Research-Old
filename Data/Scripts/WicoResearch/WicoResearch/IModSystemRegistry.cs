using Duckroll;

namespace WicoResearch
{
	public interface IModSystemRegistry
	{
		void AddCloseableModSystem(ModSystemCloseable modSystemCloseable);

		void AddUpatableModSystem(ModSystemUpdatable modSystemUpdatable);

		void AddRapidUpdatableModSystem(ModSystemRapidUpdatable modSystemRapidUpdatable);
	}
}