Timer.Repeating( "TestTimer", 1, 5, function()
    print( "TimerTest" )
end )
print( File.Exists( "Bruh" ) )