﻿using System;
using System.Collections.Generic;
using System.Text;
using Foster.Framework;

namespace Foster.GuiSystem
{
    public static class ImguiHeader
    {
        public static bool Header(this Imgui imgui, string label, bool startOpen = false)
        {
            const string StorageKey = "TOGGLED";

            var style = imgui.Style.Header;

            var id = imgui.Id(label);
            var has = imgui.Retreive(id, StorageKey, out bool toggled);
            var enabled = (has && toggled) || (!has && startOpen);

            imgui.PushSpacing(0);

            // do button behavour first
            var position = imgui.Cell(float.MaxValue, imgui.FontSize + style.Idle.Padding.Y * 2);
            if (imgui.ButtonBehaviour(id, position))
                enabled = !enabled;

            imgui.PopSpacing();

            // draw
            var inner = imgui.Box(position, style, id);
            var state = style.Current(imgui.ActiveId, imgui.HotId, id);
            var content = new Text((enabled ? "v " : "> ") + label);
            content.Draw(imgui, imgui.Batcher, state, inner);

            // store result
            imgui.Store(id, StorageKey, enabled);

            // indent tab
            if (enabled)
            {
                imgui.PushIndent(imgui.Spacing);
                imgui.PushId(id);
            }

            return enabled;
        }

        public static void EndHeader(this Imgui imgui)
        {
            imgui.Separator();
            imgui.PopIndent();
            imgui.PopId();
        }
    }
}
