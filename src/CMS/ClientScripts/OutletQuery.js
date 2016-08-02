Sys.Application.add_load(function () {
    ctrl_ProvinceIDXA.set_onSelectChanged(function (s, e) {
        ctrl_CityID4U.setUrl("CitySelect.aspx?ParaName=ProvinceID&ParaValue=" + e.Value);
    });
});

Sys.Application.add_load(function () {
    ctrl_CityID4U.set_onSelectChanged(function (s, e) {
        ctrl_DistrictIDR8.setUrl("DistrictSelect.aspx?ParaName=CityID&ParaValue=" + e.Value);
    });
});