
namespace Curry.Game
{
    public delegate void OnCharacterContextUpdate(CharacterContext c);
    public class CharacterContextFactory : IGameContextFactory<CharacterContext> 
    {
        CharacterContext m_context;
        public event OnContextUpdate<CharacterContext> OnUpdate;

        public CharacterContext Context { get { return new CharacterContext(m_context); } }

        public void UpdateContext(CharacterContext context)
        {
            m_context =  new CharacterContext(context);
            OnUpdate?.Invoke(m_context);
        }
    }
}
