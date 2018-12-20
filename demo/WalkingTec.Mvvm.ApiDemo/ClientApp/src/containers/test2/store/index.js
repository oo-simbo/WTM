var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
import StoreBasice from 'store/table';
import { message } from 'antd';
import { runInAction } from 'mobx';
var Store = /** @class */ (function (_super) {
    __extends(Store, _super);
    function Store() {
        var _this = _super.call(this) || this;
        /** 数据 ID 索引 */
        _this.IdKey = 'id';
        _this.Urls = {
            search: {
                src: "/SampleData/WeatherForecasts",
                method: "get"
            },
            details: {
                src: "/test/details/{id}",
                method: "get"
            },
            insert: {
                src: "/test/insert",
                method: "post"
            },
            update: {
                src: "/test/update",
                method: "post"
            },
            delete: {
                src: "/test/delete",
                method: "post"
            },
            import: {
                src: "/test/import",
                method: "post"
            },
            export: {
                src: "/test/export",
                method: "post"
            },
            template: {
                src: "/test/template",
                method: "post"
            }
        };
        return _this;
    }
    /**
     * 加载数据 列表
     * @param params 搜索参数
     */
    Store.prototype.onSearch = function (params, page) {
        if (params === void 0) { params = {}; }
        if (page === void 0) { page = { pageNo: 1, pageSize: 10 }; }
        return __awaiter(this, void 0, void 0, function () {
            var method, src, res;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        console.log(this);
                        if (this.pageState.loading == true) {
                            return [2 /*return*/, message.warn('数据正在加载中')];
                        }
                        this.onPageState("loading", true);
                        this.searchParams = __assign({}, this.searchParams, params);
                        params = __assign({}, page, { search: this.searchParams });
                        method = this.Urls.search.method;
                        src = this.Urls.search.src;
                        return [4 /*yield*/, this.Request[method](src, params).map(function (data) {
                                var newData = {
                                    list: []
                                };
                                // if (data.list) {
                                //     data.list = data.list.map((x, i) => {
                                //         // antd table 列表属性需要一个唯一key
                                //         return { key: i, ...x }
                                //     })
                                // }
                                newData.list = data.map(function (x, i) {
                                    // antd table 列表属性需要一个唯一key
                                    return __assign({ key: i }, x);
                                });
                                return newData;
                            }).toPromise()];
                    case 1:
                        res = _a.sent();
                        runInAction(function () {
                            _this.dataSource = res || _this.dataSource;
                            _this.onPageState("loading", false);
                        });
                        return [2 /*return*/, res];
                }
            });
        });
    };
    return Store;
}(StoreBasice));
export { Store };
export default new Store();
//# sourceMappingURL=index.js.map