using Nickel;
using System;
using System.Collections.Generic;

namespace Evil_Riggs.CardActions
{
	internal class ADiscountFirstCards : CardAction
	{
        public static Spr ADiscountFirstCardSpr;


        public required int amount;
		public int offset;
		public override void Begin(G g, State s, Combat c)
		{
			if(c.hand[offset] != null)
			{
				c.hand[offset].discount -= 1;
				if (amount > 1)
				{
					c.QueueImmediate(new ADiscountFirstCards { amount = amount-1, offset = offset+1 });
				}
			}
		}

        public override Icon? GetIcon(State s)
        {
            //return new Icon(isRandom ? StableSpr.icons_droneMoveRandom : ((dir > 0) ? StableSpr.icons_droneMoveRight : StableSpr.icons_droneMoveLeft), Math.Abs(dir), Colors.drone);
            return new(ADiscountFirstCardSpr, amount, Colors.textMain);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            return
            [
                new GlossaryTooltip(key: "ADiscountFirstCards")
                {
                    Icon = ADiscountFirstCardSpr,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "ADiscountFirstCards", "title"]),
                    TitleColor = Colors.card,
                    Description = ModEntry.Instance.Localizations.Localize(["action", "ADiscountFirstCards", "desc"], new { cnt = amount })
                },
            ];
        }
    }
}
