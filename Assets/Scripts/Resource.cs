using System;

public class Resource {
    public enum Type {
        BRICK,
        GRAIN,
        LUMBER,
        ORE,
        WOOL
    }

    public static Resource Brick() {
        return new Resource(Type.BRICK);
    }

    public static Resource Grain() {
        return new Resource(Type.GRAIN);
    }

    public static Resource Lumber() {
        return new Resource(Type.LUMBER);
    }

    public static Resource Ore() {
        return new Resource(Type.ORE);
    }

    public static Resource Wool() {
        return new Resource(Type.WOOL);
    }

    public Resource(Type type) {
        this.type = type;
    }

    public Type type {get; private set;}

    public String ToString() {
        switch (type) {
            case Type.BRICK:
                return "Brick";

            case Type.GRAIN:
                return "Grain";

            case Type.LUMBER:
                return "Lumber";

            case Type.ORE:
                return "Ore";

            case Type.WOOL:
                return "Wool";

            default:
                throw new ArgumentException();
                break;
        }
    }
}