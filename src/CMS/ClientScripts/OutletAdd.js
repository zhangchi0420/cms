Sys.Application.add_load(function () {
    ctrl_ProvinceID7R.set_onSelectChanged(function (s, e) {
        ctrl_CityIDXT.setUrl("CitySelect.aspx?ParaName=ProvinceID&ParaValue=" + e.Value);
    });
});

Sys.Application.add_load(function () {
    ctrl_CityIDXT.set_onSelectChanged(function (s, e) {
        ctrl_DistrictIDXZ.setUrl("DistrictSelect.aspx?ParaName=CityID&ParaValue=" + e.Value);
    });
});