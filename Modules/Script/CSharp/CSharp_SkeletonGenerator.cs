﻿using Vixen.Module.Script;

namespace CSharp {
	// Has to be a partial of the partial generated by the T4 (SkeletonBase) so that
	// the base can be extended with properties used by the T4 (ClassName, Namespace).
	//public class CSharp_SkeletonGenerator : SkeletonGeneratorBase {
	public partial class CSharp_Skeleton : IScriptSkeletonGenerator {
		public string Generate(string nameSpace, string className) {
			Namespace = nameSpace;
			ClassName = className;
			return TransformText();
		}

		public string Namespace { get; set; }

		public string ClassName { get; set; }
	}
}
