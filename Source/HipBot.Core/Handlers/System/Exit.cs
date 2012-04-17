﻿using System;
using System.Threading;
using HipBot.Domain;
using HipBot.Services;
using Sugar.Command;

namespace HipBot.Handlers.System
{
    /// <summary>
    /// Says hello to the sender
    /// </summary>
    public class Exit : Handler<Exit.Options>
    {
        [Flag("exit")]
        public class Options {}

        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

        /// <summary>
        /// Receives the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="room">The room.</param>
        /// <param name="options">The options.</param>
        public override void Receive(Message message, Room room, Options options)
        {
            HipChatService.Say(room, "Exiting bot.");

            Thread.Sleep(100);

            Environment.Exit(0);
        }
    }
}
