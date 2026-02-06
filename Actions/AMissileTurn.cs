using FSPRO;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evil_Riggs.CardActions
{
	internal class AMissileTurn : CardAction
	{

        public static Spr AMissileTurnSpr;

        public override void Begin(G g, State s, Combat c)
		{
            timer = 0.0;
            foreach (int itemInt in c.stuff.Keys.OrderBy((int n) => n).Reverse())
            {
				StuffBase? item = c.stuff[itemInt];
				//if (typeof(Missile) == item.GetType() || typeof(Drones.MissileLight) == item.GetType())
				if (item is Missile)
				{
					List<CardAction>? actions = item.GetActions(s, c);
					if (actions != null)
					{
						c.QueueImmediate(actions);
					}
				}
            }

            c.Queue(new ADummyAction
            {
                timer = 0.0
            });
		}

		public override List<Tooltip> GetTooltips(State s)
		{
            return
            [
                new GlossaryTooltip(key: "AMissileTurn")
                {
                    Icon = AMissileTurnSpr,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "AMissileTurn", "title"]),
                    TitleColor = Colors.card,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "AMissileTurn", "desc"])
                },
            ];
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon
            {
                path = AMissileTurnSpr,
                //number = count,
                //color = Colors.textMain
            };
        }
	}
}
