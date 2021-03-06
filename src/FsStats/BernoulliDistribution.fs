namespace FsStats

/// Bernoulli Distribution
module BernoulliDistribution =
    type T = { P : float }

    /// Creates a Bernoulli Distribution
    /// takes a probability of positive outcome.
    let create p = { P = p }

    let mean { P = p } = p

    let variance { P = p } = p * (1.0 - p)

    let stddev = sqrt << variance 

    /// Probability Mass Function
    let pmf { P = p } = function
        | 0 -> 1.0 - p
        | 1 -> p
        | _ -> 0.0

    /// Cumulative Distribution Function
    let cdf { P = p } k =
        if k < 0 then 0.0
        elif k < 1 then (1.0 - p)
        else 1.0

    /// Generates a random number from Bernoulli distribution
    let random { P = p } (rnd : System.Random) = if rnd.NextDouble() < p then 1 else 0

    /// Generates a random sample of size n from Bernoulli distribution
    let sample bd rnd n = Array.init n (fun _ -> random bd rnd)
