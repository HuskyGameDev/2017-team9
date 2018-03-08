using UnityEngine;

[System.Serializable]
public struct ColorBit {
	public Color32 color;
	public bool nulled;

	public ColorBit(Color32? color) {
		this.nulled = (color == null);
		if (nulled == false)
			this.color = (Color32)color;
		else
			this.color = default(Color32);

	}

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
	public override int GetHashCode() {
		return base.GetHashCode();
	}
	public override string ToString() {
		return nulled.ToString() + " | (" + color.r + ", " + color.g + ", " + color.b + ")";
	}
}