using System;
using System.Collections.Generic;
using System.IO;
using CustomSlot;
using CustomSlot.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using UtilitySlots14.UI;

namespace UtilitySlots14 {
    public class UtilitySlots : Mod
    {
        private static List<Func<bool>> rightClickOverrides;
        public static bool WingSlotModInstalled;
        public static bool DisableUtilitySlotModifiers = false;

        public static UtilitySlotsSystem USSystem;

        public override void Load() {
            rightClickOverrides = new List<Func<bool>>();
            if (ModLoader.TryGetMod("WingSlot", out var wingSlotInstalled))
                WingSlotModInstalled = wingSlotInstalled != null;
            USSystem = new UtilitySlotsSystem();
        }

        public override void Unload()
        {
            if(rightClickOverrides == null) return;

            rightClickOverrides.Clear();
            rightClickOverrides = null;
        }

        public override object Call(params object[] args) {
            try {
                string keyword = args[0] as string;

                if(string.IsNullOrEmpty(keyword)) {
                    return "Error: no command provided";
                }

                switch(keyword.ToLower()) {
                    case "getconfig":
                        return new Dictionary<string, object> {
                            { "AllowAccessorySlots", UtilitySlotsConfig.Instance.AllowAccessorySlots },
                            //{ "SlotLocation", UtilitySlotsConfig.Instance.SlotLocation },
                            //{ "ShowCustomLocationPanel", UtilitySlotsConfig.Instance.ShowCustomLocationPanel }
                        };
                    case "getwingequip":
                        return WingSlotModInstalled ? null : USSystem.WingUI.EquipSlot.Item;
                    case "getwingvanity":
                    case "getwingsocial":
                        return WingSlotModInstalled ? null : USSystem.WingUI.SocialSlot.Item;
                    case "getwingdye":
                        return WingSlotModInstalled ? null : USSystem.WingUI.DyeSlot.Item;
                    case "getwingvisible":
                        return WingSlotModInstalled ? null : USSystem.WingUI.SocialSlot.Item.stack > 0 ? USSystem.WingUI.SocialSlot.Item
                                                                    : USSystem.WingUI.EquipSlot.Item;
                    case "getballoonequip":
                        return USSystem.BalloonUI.EquipSlot.Item;
                    case "getballoonvanity":
                    case "getballoonsocial":
                        return USSystem.BalloonUI.SocialSlot.Item;
                    case "getballoondye":
                        return USSystem.BalloonUI.DyeSlot.Item;
                    case "getballoonvisible":
                        return USSystem.BalloonUI.SocialSlot.Item.stack > 0 ? USSystem.BalloonUI.SocialSlot.Item
                                                                    : USSystem.BalloonUI.EquipSlot.Item;
                    case "getshoeequip":
                        return USSystem.ShoeUI.EquipSlot.Item;
                    case "getshoevanity":
                    case "getshoesocial":
                        return USSystem.ShoeUI.SocialSlot.Item;
                    case "getshoedye":
                        return USSystem.ShoeUI.DyeSlot.Item;
                    case "getshoevisible":
                        return USSystem.ShoeUI.SocialSlot.Item.stack > 0 ? USSystem.ShoeUI.SocialSlot.Item
                                                                    : USSystem.ShoeUI.EquipSlot.Item;
                    case "add":
                    case "remove":
                        // wingSlot.Call(/* "add" or "remove" */, /* func<bool> returns true to cancel/false to continue */);
                        // These two should be called in PostSetupContent
                        if(!(args[1] is Func<bool> func))
                            return "Error: not a valid Func<bool>";

                        if(keyword == "add") {
                            rightClickOverrides.Add(func);
                        }
                        else {
                            rightClickOverrides.Remove(func);
                        }

                        break;
                    default:
                        return "Error: not a valid command";
                }
            }
            catch {
                return null;
            }

            return null;
        }

        //public override void HandlePacket(BinaryReader reader, int whoAmI) {
        //    PacketMessageType message = (PacketMessageType)reader.ReadByte();
        //    byte player = reader.ReadByte();
        //    WingSlotPlayer modPlayer = Main.player[player].GetModPlayer<WingSlotPlayer>();

        //    switch(message) {
        //        case PacketMessageType.All:
        //            UI.EquipSlot.SetItem(ItemIO.Receive(reader), false);
        //            UI.SocialSlot.SetItem(ItemIO.Receive(reader), false);
        //            UI.DyeSlot.SetItem(ItemIO.Receive(reader), false);

        //            if(Main.netMode == NetmodeID.Server) {
        //                ModPacket packet = GetPacket();
        //                packet.Write((byte)PacketMessageType.All);
        //                packet.Write(player);
        //                ItemIO.Send(UI.EquipSlot.Item, packet);
        //                ItemIO.Send(UI.SocialSlot.Item, packet);
        //                ItemIO.Send(UI.DyeSlot.Item, packet);
        //                packet.Send(-1, whoAmI);
        //            }
        //            break;
        //        case PacketMessageType.EquipSlot:
        //            UI.EquipSlot.SetItem(ItemIO.Receive(reader), false);
        //            if(Main.netMode == NetmodeID.Server) {
        //                modPlayer.SendSingleItemPacket(PacketMessageType.EquipSlot, UI.EquipSlot.Item, -1, whoAmI);
        //            }
        //            break;
        //        case PacketMessageType.VanitySlot:
        //            UI.SocialSlot.SetItem(ItemIO.Receive(reader), false);
        //            if(Main.netMode == NetmodeID.Server) {
        //                modPlayer.SendSingleItemPacket(PacketMessageType.VanitySlot, UI.SocialSlot.Item, -1, whoAmI);
        //            }
        //            break;
        //        case PacketMessageType.DyeSlot:
        //            UI.DyeSlot.SetItem(ItemIO.Receive(reader), false);
        //            if(Main.netMode == NetmodeID.Server) {
        //                modPlayer.SendSingleItemPacket(PacketMessageType.DyeSlot, UI.DyeSlot.Item, -1, whoAmI);
        //            }
        //            break;
        //        default:
        //            Logger.InfoFormat("[Wing Slot] Unknown message type: {0}", message);
        //            break;
        //    }
        //}

        public static bool OverrideRightClick() {
            foreach(var func in rightClickOverrides) {
                if(func()) {
                    return true;
                }
            }

            return false;
        }
    }
}
