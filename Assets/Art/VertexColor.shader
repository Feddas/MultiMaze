Shader "VertexColor" {
Subshader {
    BindChannels {
        Bind "vertex", vertex
        Bind "color", color 
    }
    Pass {}
}
}