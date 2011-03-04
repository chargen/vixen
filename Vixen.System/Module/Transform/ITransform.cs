﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vixen.Common;

namespace Vixen.Module.Transform {
	public interface ITransform {
		/// <summary>
		/// Dictionary of the commands affected by the transform according to its module
		/// descriptor and the parameters affected within that command.
		/// Set when the transform is loaded via Transform.Modules.Get.
		/// </summary>
		Dictionary<string, CommandParameterReference> CommandsAffected { get; set; }
		/// <summary>
		/// Called by the controller when a command needs to be transformed.
		/// The module needs to determine qualification on its own.
		/// </summary>
		/// <param name="command"></param>
		void Transform(CommandData command);
	}
}