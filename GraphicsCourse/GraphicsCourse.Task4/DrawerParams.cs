namespace GraphicsCourse.Task4;

public record DrawerParams(
    Segment<float> X,
    Segment<float> Y,
    Func<float, float, float> Function);