namespace FsStats

/// Poisson distribution
/// where mu is observed mean rate 
/// of independent events 
/// occurring within a fixed interval
module PoissonDistribution =
    type T = { Mu : float }

    /// Create a Poisson distribution
    let create mu = { T.Mu = mu }

    let mean { T.Mu = mu } = mu

    let variance { T.Mu = mu } = mu

    let stddev { T.Mu = mu } = sqrt mu

    let private factorial x = 
        let rec util value acc =
            if value <= 1.0 then acc
            else util (value - 1.0) (acc * value)
        util x 1.0

    let private knuthsSample mu (rnd : System.Random) =
        let l = exp (-mu)
        let rec util(k, p) =
            let p' = p * rnd.NextDouble()
            if p' > l
            then util(k+1, p')
            else k
        util(0, 1.0)
    
    /// Probability Mass Function.
    let pmf { T.Mu = mu } k = ((mu ** float k) * exp (-mu)) / (float k |> factorial)

    /// Cumulative Distribution Function.
    let cdf { T.Mu = mu } k = 
        let s = Array.sumBy (fun i -> (mu ** float i) / (float i |> factorial)) [|0 .. k|]
        exp (-mu) * s

    /// Generates a random number from Poisson distribution
    let random { T.Mu = mu } (rnd : System.Random) = knuthsSample mu rnd

    /// Generates a random sample of size m from Poisson distribution
    let sample d rnd m = Array.init m (fun _ -> random d rnd)
