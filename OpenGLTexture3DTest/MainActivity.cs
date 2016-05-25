using System;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Opengl;
using Javax.Microedition.Khronos.Opengles;
using Java.Nio;
using Android.Graphics;
using Android.Util;

namespace OpenGLTexture3DTest
{

    [Activity(Label = "OpenGLTexture3DTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, GestureDetector.IOnGestureListener
    {
        const string TAG = "调试信息";
        // 定义旋转角度
        private float angleX = 0f;
        private float angleY = 0f;
        const float ROTATE_FACTOR = 60;
        // 定义手势检测器实例
        GestureDetector detector;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // 创建一个GLSurfaceView,用于显示OpenGL绘制的图形
            GLSurfaceView glView = new GLSurfaceView(this);
            // 创建GLSurfaceView的内容绘制器
            MyRenderer myRenderer = new MyRenderer(this);
            // 为GLSurfaceView设置绘制器
            glView.SetRenderer(myRenderer);

            // Set our view from the "main" layout resource
            SetContentView(glView);
            detector = new GestureDetector(this, this);
        }

        /// <summary>
        /// 重写当前活动的触碰事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            // 将该Activity上的触碰事件交给GestureDetector处理
            // 否则手势检测器将不会起作用
            return detector.OnTouchEvent(e);
        }

        public bool OnDown(MotionEvent e)
        {
            return false;
        }

        /// <summary>
        /// 参见第50行
        /// 执行了手势检测器触碰事件后检测器自动回调这些方法
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="velocityX"></param>
        /// <param name="velocityY"></param>
        /// <returns></returns>
        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            velocityX = velocityX > 2000 ? 2000 : velocityX;
            velocityX = velocityX < -2000 ? -2000 : velocityX;
            velocityY = velocityY > 2000 ? 2000 : velocityY;
            velocityY = velocityY < -2000 ? -2000 : velocityY;
            // 根据横向上的速度计算沿Y轴旋转的角度
            angleY += velocityX * ROTATE_FACTOR / 4000;
            // 根据纵向上的速度计算沿X轴旋转的角度
            angleX += velocityY * ROTATE_FACTOR / 4000;
            return true;
        }

        public void OnLongPress(MotionEvent e)
        {
            Log.Debug(TAG, "OnLongPress正在执行");
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return false;
        }

        public void OnShowPress(MotionEvent e)
        {
            Log.Debug(TAG, "OnShowPress正在执行");
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }


        /// <summary>
        /// 继承Object类,就不用实现Dispose()方法和Handle属性
        /// </summary>
        public class MyRenderer : Java.Lang.Object, Android.Opengl.GLSurfaceView.IRenderer
        {

            // 立方体的顶点坐标（一共是36个顶点，组成12个三角形）
            private float[] cubeVertices = { -0.6f, -0.6f, -0.6f, -0.6f, 0.6f,
                -0.6f, 0.6f, 0.6f, -0.6f, 0.6f, 0.6f, -0.6f, 0.6f, -0.6f, -0.6f,
                -0.6f, -0.6f, -0.6f, -0.6f, -0.6f, 0.6f, 0.6f, -0.6f, 0.6f, 0.6f,
                0.6f, 0.6f, 0.6f, 0.6f, 0.6f, -0.6f, 0.6f, 0.6f, -0.6f, -0.6f,
                0.6f, -0.6f, -0.6f, -0.6f, 0.6f, -0.6f, -0.6f, 0.6f, -0.6f, 0.6f,
                0.6f, -0.6f, 0.6f, -0.6f, -0.6f, 0.6f, -0.6f, -0.6f, -0.6f, 0.6f,
                -0.6f, -0.6f, 0.6f, 0.6f, -0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f,
                0.6f, 0.6f, -0.6f, 0.6f, 0.6f, -0.6f, -0.6f, 0.6f, 0.6f, -0.6f,
                -0.6f, 0.6f, -0.6f, -0.6f, 0.6f, 0.6f, -0.6f, 0.6f, 0.6f, 0.6f,
                0.6f, 0.6f, 0.6f, 0.6f, -0.6f, -0.6f, 0.6f, -0.6f, -0.6f, -0.6f,
                -0.6f, -0.6f, -0.6f, 0.6f, -0.6f, -0.6f, 0.6f, -0.6f, 0.6f, 0.6f,
                -0.6f, 0.6f, -0.6f, };
            // 定义立方体所需要的6个面（一共是12个三角形所需的顶点）
            private byte[] cubeFacets = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
                13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                30, 31, 32, 33, 34, 35, };
            // 定义纹理贴图的坐标数据
            private float[] cubeTextures = { 1.0000f, 1.0000f, 1.0000f, 0.0000f,
                0.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 1.0000f, 1.0000f,
                1.0000f, 0.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.0000f,
                1.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 1.0000f, 0.0000f,
                1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.0000f, 1.0000f, 0.0000f,
                0.0000f, 0.0000f, 0.0000f, 1.0000f, 0.0000f, 1.0000f, 1.0000f,
                1.0000f, 1.0000f, 0.0000f, 1.0000f, 0.0000f, 0.0000f, 0.0000f,
                0.0000f, 1.0000f, 0.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f,
                0.0000f, 1.0000f, 0.0000f, 0.0000f, 0.0000f, 0.0000f, 1.0000f,
                0.0000f, 1.0000f, 1.0000f, 1.0000f, 1.0000f, 0.0000f, 1.0000f,
                0.0000f, 0.0000f, 0.0000f, 0.0000f, 1.0000f };
            private MainActivity ma;
            private FloatBuffer cubeVerticesBuffer;
            private ByteBuffer cubeFacetsBuffer;
            private FloatBuffer cubeTexturesBuffer;
            // 定义本程序所使用的纹理
            private int texture;

            /// <summary>
            /// 自定义的构造方法,获取主活动的引用
            /// </summary>
            /// <param name="ma"></param>
            public MyRenderer(MainActivity ma) : base()
            {
                this.ma = ma;
                // 将立方体的顶点位置数据数组包装成FloatBuffer;
                cubeVerticesBuffer = floatBufferUtil(cubeVertices);
                // 将立方体的6个面(12个三角形)的数字包装成ByteBuffer
                cubeFacetsBuffer = ByteBuffer.Wrap(cubeFacets);
                // 将立方体的纹理贴图的坐标数据包装成FloatBuffer
                cubeTexturesBuffer = floatBufferUtil(cubeTextures);
            }


            /// <summary>
            /// 定义一个工具方法，将float[]数组转换为OpenGL ES所需的FloatBuffer
            /// </summary>
            /// <param name="arr"></param>
            /// <returns></returns>
            private FloatBuffer floatBufferUtil(float[] arr)
            {
                FloatBuffer mBuffer;
                // 初始化ByteBuffer，长度为arr数组的长度*4，因为一个int占4个字节
                ByteBuffer qbb = ByteBuffer.AllocateDirect(arr.Length * 4);
                // 数组排列用nativeOrder
                qbb.Order(ByteOrder.NativeOrder());
                mBuffer = qbb.AsFloatBuffer();
                mBuffer.Put(arr);
                mBuffer.Position(0);
                return mBuffer;
            }


            public void OnDrawFrame(IGL10 gl)
            {
                // 清除屏幕缓存和深度缓存
                gl.GlClear(GL10.GlColorBufferBit | GL10.GlDepthBufferBit);
                // 启用顶点坐标数据
                gl.GlEnableClientState(GL10.GlVertexArray);
                // 启用贴图坐标数组数据
                gl.GlEnableClientState(GL10.GlTextureCoordArray);   // ①
                                                                    // 设置当前矩阵模式为模型视图。
                gl.GlMatrixMode(GL10.GlModelview);
                gl.GlLoadIdentity();
                // 把绘图中心移入屏幕2个单位
                gl.GlTranslatef(0f, 0.0f, -2.0f);
                // 旋转图形
                gl.GlRotatef(ma.angleY, 0, 1, 0);
                gl.GlRotatef(ma.angleX, 1, 0, 0);
                // 设置顶点的位置数据
                gl.GlVertexPointer(3, GL10.GlFloat, 0, cubeVerticesBuffer);
                // 设置贴图的坐标数据
                gl.GlTexCoordPointer(2, GL10.GlFloat, 0, cubeTexturesBuffer);  // ②
                                                                               // 执行纹理贴图
                gl.GlBindTexture(GL10.GlTexture2d, texture);  // ③
                                                              // 按cubeFacetsBuffer指定的面绘制三角形
                gl.GlDrawElements(GL10.GlTriangles, cubeFacetsBuffer.Remaining(),
                        GL10.GlUnsignedByte, cubeFacetsBuffer);
                // 绘制结束
                gl.GlFinish();
                // 禁用顶点、纹理坐标数组
                gl.GlDisableClientState(GL10.GlVertexArray);
                gl.GlDisableClientState(GL10.GlTextureCoordArray);
                // 递增角度值以便每次以不同角度绘制
            }


            public void OnSurfaceChanged(IGL10 gl, int width, int height)
            {
                // 设置3D视窗的大小及位置
                gl.GlViewport(0, 0, width, height);
                // 将当前矩阵模式设为投影矩阵
                gl.GlMatrixMode(GL10.GlProjection);
                // 初始化单位矩阵
                gl.GlLoadIdentity();
                // 计算透视视窗的宽度、高度比
                float ratio = (float)width / height;
                // 调用此方法设置透视视窗的空间大小。
                gl.GlFrustumf(-ratio, ratio, -1, 1, 1, 10);
            }

            public void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
            {
                // 关闭抗抖动
                gl.GlDisable(GL10.GlDither);
                // 设置系统对透视进行修正
                gl.GlHint(GL10.GlPerspectiveCorrectionHint, GL10.GlFastest);
                gl.GlClearColor(0, 0, 0, 0);
                // 设置阴影平滑模式
                gl.GlShadeModel(GL10.GlSmooth);
                // 启用深度测试
                gl.GlEnable(GL10.GlDepthTest);
                // 设置深度测试的类型
                gl.GlDepthFunc(GL10.GlLequal);
                // 启用2D纹理贴图
                gl.GlEnable(GL10.GlTexture2d);
                // 装载纹理
                loadTexture(gl);
            }


            /// <summary>
            /// 加载位图生成纹理
            /// </summary>
            /// <param name="gl"></param>
            private void loadTexture(IGL10 gl)
            {
                Bitmap bitmap = null;
                try
                {
                    // 加载位图
                    bitmap = BitmapFactory.DecodeResource(ma.Resources, Resource.Drawable.sand);
                    int[] textures = new int[1];
                    // 指定生成N个纹理（第一个参数指定生成一个纹理）
                    // textures数组将负责存储所有纹理的代号
                    gl.GlGenTextures(1, textures, 0);
                    // 获取textures纹理数组中的第一个纹理
                    texture = textures[0];
                    // 通知OpenGL将texture纹理绑定到GL10.GL_TEXTURE_2D目标中
                    gl.GlBindTexture(GL10.GlTexture2d, texture);
                    // 设置纹理被缩小（距离视点很远时被缩小）时的滤波方式
                    gl.GlTexParameterf(GL10.GlTexture2d, GL10.GlTextureMinFilter, GL10.GlNearest);
                    // 设置纹理被放大（距离视点很近时被方法）时的滤波方式
                    gl.GlTexParameterf(GL10.GlTexture2d, GL10.GlTextureMagFilter, GL10.GlLinear);
                    // 设置在横向、纵向上都是平铺纹理
                    gl.GlTexParameterf(GL10.GlTexture2d, GL10.GlTextureWrapS, GL10.GlRepeat);
                    gl.GlTexParameterf(GL10.GlTexture2d, GL10.GlTextureWrapT, GL10.GlRepeat);
                    // 加载位图生成纹理
                    GLUtils.TexImage2D(GL10.GlTexture2d, 0, bitmap, 0);
                }
                finally
                {
                    // 生成纹理之后，回收位图
                    if (bitmap != null)
                        bitmap.Recycle();
                }
            }

        }

    }
}