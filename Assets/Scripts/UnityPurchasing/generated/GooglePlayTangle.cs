// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("IFYkc24aX97qcCNw3uq2crvr51S1QXvSlmnheUJRrqdbmoMfeHtD9hS1XcP4d0gKAAtMSgtGrqgDpsX/XoKhCQiJvadGvbyqdYYw1zua1vd0ZOen33mx2Li/I3CQYuiz9aXT0r8TivDwvECB+hxU+wYyTFvLc8e7Zfbmnwnu3uqYA9QHLIt8btOQo0sTkJ6RoROQm5MTkJCRNxCG5j8mlAc29HGTLiBybrK79z8qbyk81MR3N3CDR0qM94KS+YV+Aker5RC8yV5bEcqSQgQ7P+db2NybtsYH2VvgJHUtGql4PDtU35IdYT0tlwOpi+c1oROQs6Gcl5i7F9kXZpyQkJCUkZI+ROuJpP6xTob9nU9ihRNecqMq5/ijSJE5f3+dYpOSkJGQ");
        private static int[] order = new int[] { 12,4,3,11,10,13,9,10,9,13,10,13,12,13,14 };
        private static int key = 145;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
