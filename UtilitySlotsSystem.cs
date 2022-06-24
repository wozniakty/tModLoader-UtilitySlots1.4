using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using UtilitySlots14.UI;


namespace UtilitySlots14
{
    public class UtilitySlotsSystem : ModSystem
    {

        private UserInterface wingSlotInterface;
        private UserInterface balloonSlotInterface;
        private UserInterface shoeSlotInterface;

        public WingSlotUI WingUI;
        public BalloonSlotUI BalloonUI;
        public ShoeSlotUI ShoeUI;

        public override void OnModLoad()
        {
            if (!Main.dedServ)
            {
                if (!UtilitySlots.WingSlotModInstalled)
                {
                    wingSlotInterface = new UserInterface();
                    WingUI = new WingSlotUI();
                    WingUI.Activate();
                    wingSlotInterface.SetState(WingUI);
                }

                balloonSlotInterface = new UserInterface();
                BalloonUI = new BalloonSlotUI();
                BalloonUI.Activate();
                balloonSlotInterface.SetState(BalloonUI);

                shoeSlotInterface = new UserInterface();
                ShoeUI = new ShoeSlotUI();
                ShoeUI.Activate();
                shoeSlotInterface.SetState(ShoeUI);
            }

            base.OnModLoad();
        }

        public override void Unload()
        {
            if (!UtilitySlots.WingSlotModInstalled)
                WingUI.Unload();
            BalloonUI.Unload();
            ShoeUI.Unload();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (!UtilitySlots.WingSlotModInstalled)
                if (WingUI.IsVisible)
                    wingSlotInterface?.Update(gameTime);
            if (BalloonUI.IsVisible)
                balloonSlotInterface?.Update(gameTime);
            if (ShoeUI.IsVisible)
                shoeSlotInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

            if (inventoryLayer != -1)
            {
                if (!UtilitySlots.WingSlotModInstalled)
                {
                    layers.Insert(
                        inventoryLayer,
                        new LegacyGameInterfaceLayer(
                          "Wing Slot: Custom Slot UI",
                          () =>
                          {
                              if (WingUI.IsVisible)
                                  wingSlotInterface.Draw(Main.spriteBatch, new GameTime());
                              return true;
                          },
                          InterfaceScaleType.UI));
                }
                layers.Insert(
                    inventoryLayer,
                    new LegacyGameInterfaceLayer(
                      "Balloon Slot: Custom Slot UI",
                      () => {
                          if (BalloonUI.IsVisible)
                              balloonSlotInterface.Draw(Main.spriteBatch, new GameTime());
                          return true;
                      },
                      InterfaceScaleType.UI));
                layers.Insert(
                    inventoryLayer,
                    new LegacyGameInterfaceLayer(
                      "Shoe Slot: Custom Slot UI",
                      () => {
                          if (ShoeUI.IsVisible)
                              shoeSlotInterface.Draw(Main.spriteBatch, new GameTime());
                          return true;
                      },
                      InterfaceScaleType.UI));

            }
        }
    }
}
