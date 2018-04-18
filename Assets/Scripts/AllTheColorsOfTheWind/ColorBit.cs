using UnityEngine;

[System.Serializable]
public struct ColorBit {
	public Color32 color;
	public bool nulled;

	public ColorBit(Color32? color) {
		this.nulled = (color == null);
		if (nulled == false)
			this.color = new Color32(color.Value.r, color.Value.g, color.Value.b, 255); //Create a color based on the passed one but with no transparency 
		else
			this.color = new Color32(255,255,255,255); // Pure white is the default color.

	}

	/// <summary>
	/// Override the Equals method to allow for comparision with Color32 and ColorBit
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public override bool Equals(object obj) {
		if (obj.GetType() == typeof(Color32)) {
			Color32 c = (Color32)obj;
			return (color.r == c.r) && (color.g == c.g) && (color.b == c.b);
		}
		else if (obj.GetType() == typeof(ColorBit)) {
			ColorBit c = (ColorBit)obj;
			return c.nulled == nulled && (color.r == c.color.r) && (color.g == c.color.g) && (color.b == c.color.b);
		}
		else
			return false;
	}
	/// <summary>
	/// Had to override the hashcode
	/// </summary>
	/// <returns></returns>
	public override int GetHashCode() {
		return base.GetHashCode();
	}

	/// <summary>
	/// Ovveride the ToString to give out better information on this ColorBit 
	/// </summary>
	/// <returns></returns>
	public override string ToString() {
		return "ColorBit: " + nulled.ToString() + " | (" + color.r + ", " + color.g + ", " + color.b + ")";
	}
}