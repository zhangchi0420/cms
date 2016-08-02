Sys.Application.add_load(function () {
    ctrl_ProvinceIDZG.set_onSelectChanged(function (s, e) {
        ctrl_CityIDHP.setUrl("CitySelect.aspx?ParaName=ProvinceID&ParaValue=" + e.Value);
    });
});

Sys.Application.add_load(function () {
    ctrl_CityIDHP.set_onSelectChanged(function (s, e) {
        ctrl_DistrictIDQG.setUrl("DistrictSelect.aspx?ParaName=CityID&ParaValue=" + e.Value);
    });
});