namespace Curry.Game
{
    public class StatAdderItem: ModifierItem
    {
        public virtual void Start() 
        {
            Modifier = new CharacterAdder(m_itemProperty.Name, m_modifierValue, m_duration);
        }
    }
}
