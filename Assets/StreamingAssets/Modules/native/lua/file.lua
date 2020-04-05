
file = {}
function file.Exists( file )
    local f = io.open( file, "rb" )
    if f then f:close() end
    return f != nil
end

--PushGlobal( "file", file )