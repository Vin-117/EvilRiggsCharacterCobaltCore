using FSPRO;
using Nickel;
using System.Collections.Generic;
using System.Linq;

namespace Evil_Riggs.CardActions
{
	internal class ATargetLock : CardAction
	{

		public static Spr ATargetLockSpr;


        public override void Begin(G g, State s, Combat c)
		{
			foreach (StuffBase item in c.stuff.Values.ToList())
			{
				if (item is Missile)
				{
					c.stuff.Remove(item.x);
					Missile value = new Missile
					{
						x = item.x,
						xLerped = item.xLerped,
						bubbleShield = item.bubbleShield,
						targetPlayer = item.targetPlayer,
						missileType = MissileType.seeker,
						age = item.age
					};
					c.stuff[item.x] = value;
				}
			}
			Audio.Play(Event.Drones_MissileLaunch);
			Audio.Play(Event.TogglePart);
		}

        public override List<Tooltip> GetTooltips(State s)
        {
            return
            [
                new GlossaryTooltip(key: "AMissileTurn")
                {
                    Icon = ATargetLockSpr,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "ATargetLock", "title"]),
                    TitleColor = Colors.card,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "ATargetLock", "desc"])
                },
            ];
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon
            {
                path = ATargetLockSpr,
                //number = count,
                //color = Colors.textMain
            };
        }
    }
}
