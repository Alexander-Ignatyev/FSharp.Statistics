module ``Statistical Hypothesis Tests``

open Xunit
open FsUnit.Xunit

open FsStats
open FsStats.Hypothesis


let getZTestExpectedResults lowerP =
    if lowerP < 0.5 then
        (should lessThan lowerP, should greaterThan (1.0 - lowerP), should lessThan (2.0 * lowerP))
    else
        (should greaterThan lowerP, should lessThan (1.0 - lowerP), should lessThan (2.0 * (1.0 - lowerP)))


[<Theory>]
[<InlineData(55.0, 10.0, 47, 20, 0.05)>]
[<InlineData(35.0, 10.0, 47, 20, 0.95)>]
[<InlineData(-35.0, 10.0, -47, 20, 0.05)>]
[<InlineData(-55.0, 10.0, -47, 20, 0.95)>]
let ``One-sample Z-Test Should Work`` (trueMean, trueStd, sampleMean, sampleSize, lowerP) =
    let (lowerTail, upperTail, twoTail) = getZTestExpectedResults lowerP
    OneSample.zTest trueMean trueStd sampleMean sampleSize LowerTailed |> lowerTail
    OneSample.zTest trueMean trueStd sampleMean sampleSize UpperTailed |> upperTail
    OneSample.zTest trueMean trueStd sampleMean sampleSize TwoTailed |> twoTail


[<Theory>]
[<InlineData(55.0, 10.0, "35.0, 45.0, 58.0, 44.0, 48.0", 0.05)>]
[<InlineData(35.0, 10.0, "35.0, 45.0, 58.0, 44.0, 48.0", 0.95)>]
[<InlineData(-35.0, 10.0, "-35.0, -45.0, -58.0, -44.0, -48.0", 0.05)>]
[<InlineData(-55.0, 10.0, "-35.0, -45.0, -58.0, -44.0, -48.0", 0.95)>]
let ``One-sample Z-Test for Sample Should Work`` (trueMean, trueStd, sampleString: string, lowerP) =
    let sample = sampleString.Split(',') |> Array.map float
    let (lowerTail, upperTail, twoTail) = getZTestExpectedResults lowerP
    let summary = SummaryStatistics.create sample
    let sampleMean = SummaryStatistics.mean summary
    let sampleSize = Array.length sample
    OneSample.zTest trueMean trueStd sampleMean sampleSize LowerTailed |> lowerTail
    OneSample.zTest trueMean trueStd sampleMean sampleSize UpperTailed |> upperTail
    OneSample.zTest trueMean trueStd sampleMean sampleSize TwoTailed |> twoTail


[<Theory>]
[<InlineData(55.0, "35.0, 45.0, 58.0, 44.0, 48.0", 0.05)>]
[<InlineData(35.0, "35.0, 45.0, 58.0, 44.0, 48.0", 0.95)>]
[<InlineData(-35.0, "-35.0, -45.0, -58.0, -44.0, -48.0", 0.05)>]
[<InlineData(-55.0, "-35.0, -45.0, -58.0, -44.0, -48.0", 0.95)>]
let ``One-sample T-Test for Sample Should Work`` (trueMean, sampleString: string, lowerP) =
    let sample = sampleString.Split(',') |> Array.map float
    let (lowerTail, upperTail, twoTail) = getZTestExpectedResults lowerP
    let summary = SummaryStatistics.create sample
    let sampleMean = SummaryStatistics.mean summary
    let sampleStd = SummaryStatistics.stdDev summary
    let sampleSize = Array.length sample
    OneSample.tTest trueMean sampleMean sampleStd sampleSize LowerTailed |> lowerTail
    OneSample.tTest trueMean sampleMean sampleStd sampleSize UpperTailed |> upperTail
    OneSample.tTest trueMean sampleMean sampleStd sampleSize TwoTailed |> twoTail
