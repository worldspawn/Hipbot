﻿using System;
using HipBot.Core;
using HipBot.Interfaces.Services;
using Sugar.Command;

namespace HipBot.Commands
{
    /// <summary>
    /// Lists all the rooms available.
    /// </summary>
    public class JoinRoomCommand : BoundCommand<JoinRoomCommand.Options>
    {
        public class Options
        {
            [Parameter("join", Required = true)]
            public string Room { get; set; }
        }

        /// <summary>
        /// Gets or sets the room service.
        /// </summary>
        /// <value>
        /// The room service.
        /// </value>
        public IRoomService RoomService { get; set; }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            var success = RoomService.Join(options.Room);

            if (!success)
            {
                Out.WriteLine("Unable to join {0}.", options.Room);
            }
        }
    }
}
