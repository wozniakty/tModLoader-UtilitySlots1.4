using System;
using System.Linq;
using CustomSlot;
using CustomSlot.UI;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using UtilitySlots14.UI;

namespace UtilitySlots14 {
    internal class UtilitySlotsPlayer : ModPlayer {
        public enum EquipType {
            Accessory,
            Social,
            Dye
        }
        public enum UtilityType
        {
            Wing,
            Balloon,
            Shoe
        }

        private const string WingPanelXTag = "uwingpanelx";
        private const string WingPanelYTag = "uwingpanely";
        private const string WingHiddenTag = "uwinghidden";
        private const string WingsTag = "uwings";
        private const string SocialWingsTag = "uvanitywings";
        private const string WingsDyeTag = "uwingdye";
        private const string BalloonPanelXTag = "balloonpanelx";
        private const string BalloonPanelYTag = "balloonpanely";
        private const string BalloonHiddenTag = "balloonhidden";
        private const string BalloonTag = "balloons";
        private const string SocialBalloonTag = "vanityballoons";
        private const string BalloonsDyeTag = "balloonsdye";
        private const string ShoePanelXTag = "shoepanelx";
        private const string ShoePanelYTag = "shoepanely";
        private const string ShoeHiddenTag = "shoehidden";
        private const string ShoeTag = "shoes";
        private const string SocialShoeTag = "vanityshoes";
        private const string ShoeDyeTag = "shoesdye";

        public Item EquippedWings { get; set; }
        public Item SocialWings { get; set; }
        public Item WingsDye { get; set; }
        public bool WingsVisible { get; set; }
        public Item EquippedBalloons { get; set; }
        public Item SocialBalloons { get; set; }
        public Item BalloonsDye { get; set; }
        public bool BalloonsVisible { get; set; }
        public Item EquippedShoes { get; set; }
        public Item SocialShoes { get; set; }
        public Item ShoesDye { get; set; }
        public bool ShoesVisible { get; set; }

        public override void Initialize() {
            if (!UtilitySlots.WingSlotModInstalled)
            {
                EquippedWings = new Item();
                SocialWings = new Item();
                WingsDye = new Item();
                WingsVisible = true;
                EquippedWings.SetDefaults();
                SocialWings.SetDefaults();
                WingsDye.SetDefaults();
            }

            EquippedBalloons = new Item();
            SocialBalloons = new Item();
            BalloonsDye = new Item();
            BalloonsVisible = true;
            EquippedBalloons.SetDefaults();
            SocialBalloons.SetDefaults();
            BalloonsDye.SetDefaults();

            EquippedShoes = new Item();
            SocialShoes = new Item();
            ShoesDye = new Item();
            ShoesVisible = true;
            EquippedShoes.SetDefaults();
            SocialShoes.SetDefaults();
            ShoesDye.SetDefaults();
        }

        public override void OnEnterWorld(Player player) {
            if (!UtilitySlots.WingSlotModInstalled)
            {
                EquipItem(EquippedWings, UtilityType.Wing, EquipType.Accessory, false);
                EquipItem(SocialWings, UtilityType.Wing, EquipType.Social, false);
                EquipItem(WingsDye, UtilityType.Wing, EquipType.Dye, false);
                UtilitySlots.USSystem.WingUI.EquipSlot.ItemVisible = WingsVisible;
            }

            EquipItem(EquippedBalloons, UtilityType.Balloon, EquipType.Accessory, false);
            EquipItem(SocialBalloons, UtilityType.Balloon, EquipType.Social, false);
            EquipItem(BalloonsDye, UtilityType.Balloon, EquipType.Dye, false);
            UtilitySlots.USSystem.BalloonUI.EquipSlot.ItemVisible = BalloonsVisible;

            EquipItem(EquippedShoes, UtilityType.Shoe, EquipType.Accessory, false);
            EquipItem(SocialShoes, UtilityType.Shoe, EquipType.Social, false);
            EquipItem(ShoesDye, UtilityType.Shoe, EquipType.Dye, false);
            UtilitySlots.USSystem.ShoeUI.EquipSlot.ItemVisible = ShoesVisible;
        }

        public override void clientClone(ModPlayer clientClone) {
            UtilitySlotsPlayer clone = clientClone as UtilitySlotsPlayer;

            if(clone == null) {
                return;
            }
            if (!UtilitySlots.WingSlotModInstalled)
            {
                clone.EquippedWings = EquippedWings.Clone();
                clone.SocialWings = SocialWings.Clone();
                clone.WingsDye = WingsDye.Clone();
            }

            clone.EquippedBalloons = EquippedBalloons.Clone();
            clone.SocialBalloons = SocialBalloons.Clone();
            clone.BalloonsDye = BalloonsDye.Clone();

            clone.EquippedShoes = EquippedShoes.Clone();
            clone.SocialShoes = SocialShoes.Clone();
            clone.ShoesDye = ShoesDye.Clone();
        }

        //public override void SendClientChanges(ModPlayer clientPlayer) {
        //    WingSlotPlayer oldClone = clientPlayer as WingSlotPlayer;

        //    if(oldClone == null) {
        //        return;
        //    }

        //    if(oldClone.EquippedWings.IsNotTheSameAs(EquippedWings)) {
        //        SendSingleItemPacket(PacketMessageType.EquipSlot, EquippedWings, -1, player.whoAmI);
        //    }

        //    if(oldClone.SocialWings.IsNotTheSameAs(SocialWings)) {
        //        SendSingleItemPacket(PacketMessageType.VanitySlot, SocialWings, -1, player.whoAmI);
        //    }

        //    if(oldClone.WingsDye.IsNotTheSameAs(WingsDye)) {
        //        SendSingleItemPacket(PacketMessageType.DyeSlot, WingsDye, -1, player.whoAmI);
        //    }
        //}

        //internal void SendSingleItemPacket(PacketMessageType message, Item item, int toWho, int fromWho) {
        //    ModPacket packet = mod.GetPacket();
        //    packet.Write((byte)message);
        //    packet.Write((byte)player.whoAmI);
        //    ItemIO.Send(item, packet);
        //    packet.Send(toWho, fromWho);
        //}

        // TODO: fix sending packets to other players
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)PacketMessageType.All);
            packet.Write((byte)Player.whoAmI);
            if (!UtilitySlots.WingSlotModInstalled)
            {
                ItemIO.Send(EquippedWings, packet);
                ItemIO.Send(SocialWings, packet);
                ItemIO.Send(WingsDye, packet);
            }
            ItemIO.Send(EquippedBalloons, packet);
            ItemIO.Send(SocialBalloons, packet);
            ItemIO.Send(BalloonsDye, packet);
            ItemIO.Send(EquippedShoes, packet);
            ItemIO.Send(SocialShoes, packet);
            ItemIO.Send(ShoesDye, packet);
            packet.Send(toWho, fromWho);
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
            if (!UtilitySlots.WingSlotModInstalled)
            {
                if (WingsDye.stack > 0 && (EquippedWings.stack > 0 || SocialWings.stack > 0))
                {
                    drawInfo.cWings = WingsDye.dye;
                }
            }
            if (BalloonsDye.stack > 0 && (EquippedBalloons.stack > 0 || SocialBalloons.stack > 0))
            {
                drawInfo.cBalloon = BalloonsDye.dye;
            }
            if (ShoesDye.stack > 0 && (EquippedShoes.stack > 0 || SocialShoes.stack > 0))
            {
                drawInfo.cShoe = ShoesDye.dye;
            }
        }

        /// <summary>
        /// Update player with the equipped wings.
        /// </summary>
        public override void UpdateEquips() {
            if (!UtilitySlots.WingSlotModInstalled)
            {
                if (UtilitySlots.USSystem.WingUI != null)
                {
                    if (EquippedWings.stack > 0)
                    {
                        Player.VanillaUpdateAccessory(Player.whoAmI, EquippedWings, !WingsVisible);
                        Player.VanillaUpdateEquip(EquippedWings);
                    }


                    if (SocialWings.stack > 0)
                    {
                        Player.VanillaUpdateVanityAccessory(SocialWings);
                    }
                }
            }
            if (UtilitySlots.USSystem.BalloonUI != null)
            {
                if (EquippedBalloons.stack > 0)
                {
                    Player.VanillaUpdateAccessory(Player.whoAmI, EquippedBalloons, !BalloonsVisible, ref wallSpeedBuff,
                                                  ref tileSpeedBuff, ref tileRangeBuff);
                    Player.VanillaUpdateEquip(EquippedBalloons);
                }

                if (SocialBalloons.stack > 0)
                {
                    Player.VanillaUpdateVanityAccessory(SocialBalloons);
                }
            }
            if (UtilitySlots.USSystem.ShoeUI != null)
            {
                if (EquippedShoes.stack > 0)
                {
                    Player.VanillaUpdateAccessory(Player.whoAmI, EquippedShoes, !ShoesVisible, ref wallSpeedBuff,
                                                  ref tileSpeedBuff, ref tileRangeBuff);
                    Player.VanillaUpdateEquip(EquippedShoes);
                }

                if (SocialShoes.stack > 0)
                {
                    Player.VanillaUpdateVanityAccessory(SocialShoes);
                }
            }
        }

        /// <summary>
        /// Since there is no tModLoader hook in UpdateDyes, we use PreUpdateBuffs which is right after that.
        /// </summary>
        public override void PreUpdateBuffs() {
            if(!UtilitySlots.WingSlotModInstalled && UtilitySlots.USSystem.WingUI != null)
            {
                if (WingsDye.stack > 0 && (SocialWings.stack > 0 || (EquippedWings.stack > 0 && WingsVisible)))
                    Player.cWings = WingsDye.dye;
            }

            if(UtilitySlots.USSystem.BalloonUI != null)
                if (BalloonsDye.stack > 0 && (SocialBalloons.stack > 0 || (EquippedBalloons.stack > 0 && BalloonsVisible)))
                    Player.cBalloon = BalloonsDye.dye;

            if(UtilitySlots.USSystem.ShoeUI != null)
                if (ShoesDye.stack > 0 && (SocialShoes.stack > 0 || (EquippedShoes.stack > 0 && ShoesVisible)))
                    Player.cShoe = ShoesDye.dye;
        }

        /// <summary>
        /// Drop items if the player character is Medium or Hardcore.
        /// </summary>
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
            if(Player.difficulty == 0) return;
            var source = Player.GetSource_Death();

            if (!UtilitySlots.WingSlotModInstalled)
            {
                Player.QuickSpawnClonedItem(source, EquippedWings);
                Player.QuickSpawnClonedItem(source, SocialWings);
                Player.QuickSpawnClonedItem(source, WingsDye);
                EquipItem(new Item(), UtilityType.Wing, EquipType.Accessory, false);
                EquipItem(new Item(), UtilityType.Wing, EquipType.Social, false);
                EquipItem(new Item(), UtilityType.Wing, EquipType.Dye, false);
            }

            Player.QuickSpawnClonedItem(source, EquippedBalloons);
            Player.QuickSpawnClonedItem(source, SocialBalloons);
            Player.QuickSpawnClonedItem(source, BalloonsDye);
            EquipItem(new Item(), UtilityType.Balloon, EquipType.Accessory, false);
            EquipItem(new Item(), UtilityType.Balloon, EquipType.Social, false);
            EquipItem(new Item(), UtilityType.Balloon, EquipType.Dye, false);

            Player.QuickSpawnClonedItem(source, EquippedShoes);
            Player.QuickSpawnClonedItem(source, SocialShoes);
            Player.QuickSpawnClonedItem(source, ShoesDye);
            EquipItem(new Item(), UtilityType.Shoe, EquipType.Accessory, false);
            EquipItem(new Item(), UtilityType.Shoe, EquipType.Social, false);
            EquipItem(new Item(), UtilityType.Shoe, EquipType.Dye, false);
        }

        /// <summary>
        /// Save player settings.
        /// </summary>
        public override void SaveData(TagCompound tag) {
            tag[BalloonPanelXTag] = UtilitySlots.USSystem.BalloonUI?.PanelCoordinates.X;
            tag[BalloonPanelYTag] = UtilitySlots.USSystem.BalloonUI?.PanelCoordinates.Y;
            tag[BalloonHiddenTag] = BalloonsVisible;
            tag[BalloonTag] = ItemIO.Save(EquippedBalloons);
            tag[SocialBalloonTag] = ItemIO.Save(SocialBalloons);
            tag[BalloonsDyeTag] = ItemIO.Save(BalloonsDye);

            tag[ShoePanelXTag] = UtilitySlots.USSystem.ShoeUI?.PanelCoordinates.X;
            tag[ShoePanelYTag] = UtilitySlots.USSystem.ShoeUI?.PanelCoordinates.Y;
            tag[ShoeHiddenTag] = ShoesVisible;
            tag[ShoeTag] = ItemIO.Save(EquippedShoes);
            tag[SocialShoeTag] = ItemIO.Save(SocialShoes);
            tag[ShoeDyeTag] = ItemIO.Save(ShoesDye);
            if (!UtilitySlots.WingSlotModInstalled)
            {
                tag[WingPanelXTag] = UtilitySlots.USSystem.WingUI?.PanelCoordinates.X;
                tag[WingPanelYTag] = UtilitySlots.USSystem.WingUI?.PanelCoordinates.Y;
                tag[WingHiddenTag] = WingsVisible;
                tag[WingsTag] = ItemIO.Save(EquippedWings);
                tag[SocialWingsTag] = ItemIO.Save(SocialWings);
                tag[WingsDyeTag] = ItemIO.Save(WingsDye);
            }
        }

        /// <summary>
        /// Load the mod settings.
        /// </summary>
        public override void LoadData(TagCompound tag) {
            if (!UtilitySlots.WingSlotModInstalled)
            {
                if (tag.ContainsKey(WingsTag))
                    EquippedWings = ItemIO.Load(tag.GetCompound(WingsTag));

                if (tag.ContainsKey(SocialWingsTag))
                    SocialWings = ItemIO.Load(tag.GetCompound(SocialWingsTag));

                if (tag.ContainsKey(WingsDyeTag))
                    WingsDye = ItemIO.Load(tag.GetCompound(WingsDyeTag));

                if (tag.ContainsKey(WingHiddenTag))
                    WingsVisible = tag.GetBool(WingHiddenTag);

                if (tag.ContainsKey(WingPanelXTag))
                    UtilitySlots.USSystem.WingUI.PanelCoordinates.X = tag.GetFloat(WingPanelXTag);

                if (tag.ContainsKey(WingPanelYTag))
                    UtilitySlots.USSystem.WingUI.PanelCoordinates.Y = tag.GetFloat(WingPanelYTag);
            }


            if (tag.ContainsKey(BalloonTag))
                EquippedBalloons = ItemIO.Load(tag.GetCompound(BalloonTag));

            if (tag.ContainsKey(SocialBalloonTag))
                SocialBalloons = ItemIO.Load(tag.GetCompound(SocialBalloonTag));

            if (tag.ContainsKey(BalloonsDyeTag))
                BalloonsDye = ItemIO.Load(tag.GetCompound(BalloonsDyeTag));

            if (tag.ContainsKey(BalloonHiddenTag))
                BalloonsVisible = tag.GetBool(BalloonHiddenTag);

            if (tag.ContainsKey(BalloonPanelXTag))
                UtilitySlots.USSystem.BalloonUI.PanelCoordinates.X = tag.GetFloat(BalloonPanelXTag);

            if (tag.ContainsKey(BalloonPanelYTag))
                UtilitySlots.USSystem.BalloonUI.PanelCoordinates.Y = tag.GetFloat(BalloonPanelYTag);


            if (tag.ContainsKey(ShoeTag))
                EquippedShoes = ItemIO.Load(tag.GetCompound(ShoeTag));

            if (tag.ContainsKey(SocialShoeTag))
                SocialShoes = ItemIO.Load(tag.GetCompound(SocialShoeTag));

            if (tag.ContainsKey(ShoeDyeTag))
                ShoesDye = ItemIO.Load(tag.GetCompound(ShoeDyeTag));

            if (tag.ContainsKey(ShoeHiddenTag))
                ShoesVisible = tag.GetBool(ShoeHiddenTag);

            if (tag.ContainsKey(ShoePanelXTag))
                UtilitySlots.USSystem.ShoeUI.PanelCoordinates.X = tag.GetFloat(ShoePanelXTag);

            if (tag.ContainsKey(ShoePanelYTag))
                UtilitySlots.USSystem.ShoeUI.PanelCoordinates.Y = tag.GetFloat(ShoePanelYTag);
        }

        /// <summary>
        /// Equip either wings or a dye in a slot.
        /// </summary>
        /// <param name="item">item to equip</param>
        /// <param name="utilityType">which utility slot to equip in</param>
        /// <param name="type">what type of slot to equip in</param>
        /// <param name="fromInventory">whether the item is being equipped from the inventory</param>
        public void EquipItem(Item item, UtilityType utilityType, EquipType type, bool fromInventory) {
            if(item == null) return;
            if (UtilitySlots.DisableUtilitySlotModifiers)
                item.prefix = 0;

            var slot = GetSlot(utilityType, type);

            if(type == EquipType.Dye) {
                if(utilityType == UtilityType.Wing)
                    if(WingsDye.IsNotSameTypePrefixAndStack(item))
                        WingsDye = item.Clone();
                if (utilityType == UtilityType.Balloon)
                    if (BalloonsDye.IsNotSameTypePrefixAndStack(item))
                        BalloonsDye = item.Clone();
                if (utilityType == UtilityType.Shoe)
                    if (ShoesDye.IsNotSameTypePrefixAndStack(item))
                        ShoesDye = item.Clone();
            }
            else if(type == EquipType.Social) {
                if(utilityType == UtilityType.Wing)
                    if(SocialWings.IsNotSameTypePrefixAndStack(item))
                        SocialWings = item.Clone();
                if (utilityType == UtilityType.Balloon)
                    if (SocialBalloons.IsNotSameTypePrefixAndStack(item))
                        SocialBalloons = item.Clone();
                if (utilityType == UtilityType.Shoe)
                    if (SocialShoes.IsNotSameTypePrefixAndStack(item))
                        SocialShoes = item.Clone();
            }
            else {
                if(utilityType == UtilityType.Wing)
                    if(EquippedWings.IsNotSameTypePrefixAndStack(item))
                        EquippedWings = item.Clone();
                if (utilityType == UtilityType.Balloon)
                    if (EquippedBalloons.IsNotSameTypePrefixAndStack(item))
                        EquippedBalloons = item.Clone();
                if (utilityType == UtilityType.Shoe)
                    if (EquippedShoes.IsNotSameTypePrefixAndStack(item))
                        EquippedShoes = item.Clone();
            }

            if(fromInventory) {
                item.favorited = false;

                int fromSlot = Array.FindIndex(Player.inventory, i => i == item);

                if(fromSlot < 0) return;

                Player.inventory[fromSlot] = slot.Item.Clone();
                SoundEngine.PlaySound(SoundID.Grab);
                Recipe.FindRecipes();
            }

            slot.SetItem(item);
        }

        private CustomItemSlot GetSlot(UtilityType utilityType, EquipType equipType)
        {
            UtilitySlotUI uiSlot = utilityType switch {
                UtilityType.Wing => UtilitySlots.USSystem.WingUI,
                UtilityType.Balloon => UtilitySlots.USSystem.BalloonUI,
                UtilityType.Shoe => UtilitySlots.USSystem.ShoeUI,
                _ => throw new ArgumentOutOfRangeException(nameof(utilityType))
            };

            return equipType switch
            {
                EquipType.Accessory => uiSlot.EquipSlot,
                EquipType.Social => uiSlot.SocialSlot,
                EquipType.Dye => uiSlot.DyeSlot,
                _ => throw new ArgumentOutOfRangeException(nameof(equipType))
            };
        }

        /// <summary>
        /// Fires when the item in a slot is changed.
        /// </summary>
        public void ItemChanged(CustomItemSlot slot, ItemChangedEventArgs e) {
            var type = GetTypeFromTag(slot);

            if (type == UtilityType.Wing)
                if (slot.Context == ItemSlot.Context.EquipAccessory)
                    EquippedWings = e.NewItem.Clone();
                else if(slot.Context == ItemSlot.Context.EquipAccessoryVanity)
                    SocialWings = e.NewItem.Clone();
                else
                    WingsDye = e.NewItem.Clone();

            if (type == UtilityType.Balloon)
                if (slot.Context == ItemSlot.Context.EquipAccessory)
                    EquippedBalloons = e.NewItem.Clone();
                else if (slot.Context == ItemSlot.Context.EquipAccessoryVanity)
                    SocialBalloons = e.NewItem.Clone();
                else
                    BalloonsDye = e.NewItem.Clone();


            if (type == UtilityType.Shoe)
                if (slot.Context == ItemSlot.Context.EquipAccessory)
                    EquippedShoes = e.NewItem.Clone();
                else if (slot.Context == ItemSlot.Context.EquipAccessoryVanity)
                    SocialShoes = e.NewItem.Clone();
                else
                    ShoesDye = e.NewItem.Clone();
        }

        private UtilityType GetTypeFromTag(CustomItemSlot slot) =>
            slot.Tag switch
            {
                "WingSlot" => UtilityType.Wing,
                "BalloonSlot" => UtilityType.Balloon,
                "ShoeSlot" => UtilityType.Shoe,
                _ => throw new NotImplementedException()
            };

        /// <summary>
        /// Fires when the visibility of an item in a slot is toggled.
        /// </summary>
        public void ItemVisibilityChanged(CustomItemSlot slot, ItemVisibilityChangedEventArgs e) {
            var type = GetTypeFromTag(slot);
            if(type == UtilityType.Wing)
                WingsVisible = e.Visibility;
            if (type == UtilityType.Balloon)
                BalloonsVisible = e.Visibility;
            if (type == UtilityType.Shoe)
                ShoesVisible = e.Visibility;
        }
    }
}
