namespace FsStats

open FsStats.Special

/// Standard Normal Distribution
module StandardDistribution =
    let rec private nextPair (rnd: System.Random) =
        let u = rnd.NextDouble() * 2.0 - 1.0
        let v = rnd.NextDouble() * 2.0 - 1.0
        let s = u*u + v*v
        if s > 0.0 && s < 1.0
        then (u, v)
        else nextPair rnd


    /// Marsaglia polar sampling method
    /// for generating a pair of independent 
    /// standard normal random variables
    let private polarMethod (rnd: System.Random) =
        let u, v = nextPair rnd
        let s = u*u + v*v
        let a = sqrt ((-2.0 * log s)/ s)
        (u*a, v*a)

    let mean = 0.0

    let variance = 1.0

    let stddev = 1.0

    let cdf x =
        (1.0 + erf(x / (sqrt 2.0))) * 0.5

    /// Generates a random number from Standard Normal distribution
    let random (rnd : System.Random) =
        let x, _ = polarMethod rnd
        x

    /// Generates a random sample of size m from Standard Normal distribution
    let sample rnd m = Array.init m (fun _ -> random rnd)
