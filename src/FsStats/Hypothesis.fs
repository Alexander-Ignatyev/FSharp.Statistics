namespace FsStats

module Hypothesis =
    type TestType =
        | LowerTailed
        | UpperTailed
        | TwoTailed


    // One-sample tests
    module OneSample =
        let private performTest cdf testType score =
            let p = cdf -(abs score)
            let pValue = match testType with
                         | LowerTailed -> if score < 0.0 then p else 1.0 - p
                         | UpperTailed -> if score < 0.0 then 1.0 - p else p
                         | TwoTailed -> 2.0 * p
            pValue

        /// Perform Z-Test for the given true mean, true standard deviation, sample mean and and size of the sample.  
        let zTest trueMean trueStd sampleMean sampleSize testType =
            let standardError = trueStd / sqrt (float sampleSize)
            let z = (sampleMean - trueMean) / standardError
            performTest StandardDistribution.cdf testType z

        /// Perform Student's T-Test for the given true mean, sample mean, sample standard deviation and size of the sample.
        let tTest trueMean sampleMean sampleStd sampleSize testType =
            let standardError = sampleStd / sqrt (sampleSize |> float)
            let tScore = (sampleMean - trueMean) / standardError
            performTest (StudentsDistribution.cdf (sampleSize - 1)) testType tScore

